using System.Linq;
using Common.Validators;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AutoNMock.Validators
{
    // ToDo_AP: добавить проверки на нулл (нужно узнать, как подключать решарперы)
    // ToDo_AP: класс можно сделать универсальным
    internal sealed class IsClassHasTestClassAttribute : IValidator<IClassDeclaration>
    {
        public bool Validate(IClassDeclaration classDeclaration)
        {
            return classDeclaration.Attributes.Any(o => o.Name.ShortName == "TestClass");
        }
    }
}