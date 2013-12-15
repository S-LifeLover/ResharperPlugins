using System.Linq;
using Common.Validators;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AutoNMock.Validators
{
    // ToDo_AP: добавить проверки на нулл (нужно узнать, как подключать решарперы)
    // ToDo_AP: Класс можно сделать универсальным
    internal sealed class IsClassContainsMocksClass : IValidator<IClassDeclaration>
    {
        public bool Validate(IClassDeclaration classDeclaration)
        {
            return classDeclaration.NestedTypeDeclarations.Any(o => o.CLRName == "Mocks");
        }
    }
}