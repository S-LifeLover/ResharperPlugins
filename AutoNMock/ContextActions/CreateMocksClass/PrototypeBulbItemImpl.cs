using System;
using System.IO;
using System.Linq;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.TextControl;

namespace AutoNMock.ContextActions.CreateMocksClass
{
    // ToDO_AP: Прототип логики. В последствии должен быть полностью переписан.
    public class PrototypeBulbItemImpl : BulbItemImpl
    {
        public PrototypeBulbItemImpl(IContextActionDataProvider contextActionDataProvider)
        {
            _contextActionDataProvider = contextActionDataProvider;
        }

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            File.WriteAllText(@"D:\Temp\PluginLog.txt", "All good");

            try
            {
                var classDeclaration = _contextActionDataProvider.GetSelectedElement<IClassDeclaration>(false, false);

                var factory = CSharpElementFactory.GetInstance(_contextActionDataProvider.PsiModule);


                const string mocksClassTemplate = @"
                        private sealed class Mocks
                        {{
                            public Mocks(MockFactory mockFactory)
                            {{
                                {3}
                                Sut = new {0}({1});
                            }}
                        
                            public {0} Sut {{ get; private set; }}

                            {2}
                        }}";

                const string mocksClassDependencyTemplate = @"public Mock<{0}> {1} {{ get; private set; }}";

                const string mocksClassDependencyInitializationTemplate = @"{0} = mockFactory.CreateMock<{1}>();";

                var sutDeclaration = _contextActionDataProvider.GetSelectedElement<IVariableDeclaration>(false, true);
                if (sutDeclaration == null)
                {
                    return null;
                }

                if (!sutDeclaration.Type.GetScalarType().GetTypeElement().Constructors.Any())
                {
                    return null;
                }

                var constructor = sutDeclaration.Type.GetScalarType().GetTypeElement().Constructors.First();

                var dependenciesInitializations = string.Empty;
                foreach (var parameter in constructor.Parameters)
                {
                    dependenciesInitializations += string.Format(
                        mocksClassDependencyInitializationTemplate,
                        ToUpperFirstLetter(parameter.ShortName),
                        parameter.Type.GetScalarType().GetClrName().ShortName) + "\n";
                }

                var dependencies = string.Empty;
                foreach (var parameter in constructor.Parameters)
                {
                    dependencies += string.Format(
                        mocksClassDependencyTemplate,
                        parameter.Type.GetScalarType().GetClrName().ShortName,
                        ToUpperFirstLetter(parameter.ShortName)) + "\n\n";
                }

                var parametersString = string.Join(", ", constructor.Parameters.Select(o => "\n" + ToUpperFirstLetter(o.ShortName) + ".MockObject"));

                var mocksClass =
                    string.Format(
                    mocksClassTemplate,
                    sutDeclaration.Type.GetScalarType().GetClrName().ShortName,
                    parametersString,
                    dependencies,
                    dependenciesInitializations);

                var memberDeclaration = factory.CreateTypeMemberDeclaration(mocksClass) as IClassDeclaration;
                classDeclaration.AddClassMemberDeclaration(memberDeclaration);
            }
            catch (Exception exception)
            {
                File.WriteAllText(@"D:\Temp\PluginLog.txt", exception.ToString());
            }

            return null;
        }

        public override string Text
        {
            get { return "Add Mocks class"; }
        }

        private readonly IContextActionDataProvider _contextActionDataProvider;

        private static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            char[] letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }
    }
}
