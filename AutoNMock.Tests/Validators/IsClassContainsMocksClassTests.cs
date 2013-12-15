using AutoNMock.Validators;
using Common.Tests.TestTools;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoNMock.Tests.Validators
{
    // ToDO_AP: устранить дублирование
    [TestClass]
    public class IsClassContainsMocksClassTests : TestsWithMocks
    {
        [TestMethod]
        public void ReturnTrueIfClassContainsMocksClass()
        {
            var sut = new IsClassContainsMocksClass();
            var classDeclaration = CreateClassDeclarationWithNestedClass("Mocks");
            Assert.IsTrue(sut.Validate(classDeclaration));
        }

        [TestMethod]
        public void ReturnFalseIfClassNotContainsAnyClasses()
        {
            var sut = new IsClassContainsMocksClass();

            var classDeclaration = MockFactory.CreateMock<IClassDeclaration>();
            classDeclaration.Expects.One.GetProperty(o => o.NestedTypeDeclarations)
                .WillReturn(new TreeNodeCollection<ICSharpTypeDeclaration>(new ICSharpTypeDeclaration[] { }));
            Assert.IsFalse(sut.Validate(classDeclaration.MockObject));
        }

        [TestMethod]
        public void ReturnFalseIfClassNotContainsMocksClass()
        {
            var sut = new IsClassContainsMocksClass();
            var classDeclaration = CreateClassDeclarationWithNestedClass("NotMocks");
            Assert.IsFalse(sut.Validate(classDeclaration));
        }

        private IClassDeclaration CreateClassDeclarationWithNestedClass(string nestedClassName)
        {
            var classDeclaration = MockFactory.CreateMock<IClassDeclaration>();
            var typeDeclaration = MockFactory.CreateMock<ICSharpTypeDeclaration>();
            classDeclaration.Expects.One.GetProperty(o => o.NestedTypeDeclarations)
                .WillReturn(new TreeNodeCollection<ICSharpTypeDeclaration>(new[] { typeDeclaration.MockObject }));
            typeDeclaration.Expects.One.GetProperty(o => o.CLRName).WillReturn(nestedClassName);
            return classDeclaration.MockObject;
        }
    }
}
