using AutoNMock.Validators;
using Common.Tests.TestTools;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoNMock.Tests.Validators
{
    [TestClass]
    public class IsClassHasTestClassAttributeTests : TestsWithMocks
    {
        [TestMethod]
        public void ReturnTrueIfMethodHasTestMethodAttribute()
        {
            var classDeclaration = CreateClassDeclarationMock("TestClass");
            var sut = new IsClassHasTestClassAttribute();
            Assert.IsTrue(sut.Validate(classDeclaration));
        }

        [TestMethod]
        public void ReturnFalseIfMethodHasNotTestMethodAttribute()
        {
            var classDeclaration = CreateClassDeclarationMock("NotTestClass");
            var sut = new IsClassHasTestClassAttribute();
            Assert.IsFalse(sut.Validate(classDeclaration));
        }

        private IClassDeclaration CreateClassDeclarationMock(string className)
        {
            var attribute = MockFactory.CreateMock<IAttribute>();
            var attributes = new TreeNodeCollection<IAttribute>(new[] { attribute.MockObject });
            var classDeclaration = MockFactory.CreateMock<IClassDeclaration>();
            classDeclaration.Expects.One.GetProperty(o => o.Attributes).WillReturn(attributes);
            var referenceName = MockFactory.CreateMock<IReferenceName>();
            attribute.Expects.One.GetProperty(o => o.Name).WillReturn(referenceName.MockObject);
            referenceName.Expects.One.GetProperty(o => o.ShortName).WillReturn(className);
            return classDeclaration.MockObject;
        }
    }
}
