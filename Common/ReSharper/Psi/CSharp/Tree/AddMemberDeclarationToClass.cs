using Common.Executors;
using Common.Factories;
using Common.Providers;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace Common.ReSharper.Psi.CSharp.Tree
{
    // ToDO_AP: Подумать, куда бы перенести этот класс, так как он общий
    internal class AddMemberDeclarationToClass : IExecutor
    {
        // ToDO_AP: надо бы подумать, в какой последовательности лучше задавать параметры конструктора
        public AddMemberDeclarationToClass(
            INullableValueProvider<IClassDeclaration> classProvider,
            IFactory<IClassDeclaration> classMemberDeclarationFactory)
        {
            _classProvider = classProvider;
            _classMemberDeclarationFactory = classMemberDeclarationFactory;
        }

        // ToDo_AP: Подумать, что сделать с обработкой ошибок (в частности, если selectedClass нулл)
        public void Execute()
        {
            var mocksClass = _classMemberDeclarationFactory.Create();
            var classObj = _classProvider.GetValueOrDefault();
            if (classObj != null)
                classObj.AddClassMemberDeclaration(mocksClass);
        }

        private readonly INullableValueProvider<IClassDeclaration> _classProvider;
        private readonly IFactory<IClassDeclaration> _classMemberDeclarationFactory;
    }
}
