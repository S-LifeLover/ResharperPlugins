using System.Linq;
using Common.Validators;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace AutoNMock.Validators
{
    // ToDo_AP: �������� �������� �� ���� (����� ������, ��� ���������� ���������)
    // ToDo_AP: ����� ����� ������� �������������
    internal sealed class IsClassHasTestClassAttribute : IValidator<IClassDeclaration>
    {
        public bool Validate(IClassDeclaration classDeclaration)
        {
            return classDeclaration.Attributes.Any(o => o.Name.ShortName == "TestClass");
        }
    }
}