using AutoNMock.Validators;
using Common.Providers;
using Common.Tests.TestTools;
using Common.Validators;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace AutoNMock.Tests.Validators
{
    // ToDO_AP: устранить дублирование
    [TestClass]
    public class CreateMocksClassContextActionAvailabilityValidatorTests : TestsWithMocks
    {
        [TestMethod]
        public void FalseIfVariableNotSelected()
        {
            var mocks = new Mocks(MockFactory);

            mocks.SelectedVariableProviderExpectGetValueOrDefault(null);
            mocks.IsVariableNotNullExpectValidate(null, false);

            Assert.IsFalse(mocks.Sut.Validate());
        }

        [TestMethod]
        public void FalseIfClassHasNotTestAttribute()
        {
            var mocks = new Mocks(MockFactory);

            var variableDeclaration = MockFactory.CreateMock<IVariableDeclaration>();
            mocks.SelectedVariableProviderExpectGetValueOrDefault(variableDeclaration.MockObject);
            mocks.IsVariableNotNullExpectValidate(variableDeclaration.MockObject, true);

            var classDeclaration = MockFactory.CreateMock<IClassDeclaration>();
            mocks.SelectedClassProviderExpectGetValueOrDefault(classDeclaration.MockObject);
            mocks.IsClassHasTestClassAttributeExpectValidate(classDeclaration.MockObject, false);

            Assert.IsFalse(mocks.Sut.Validate());
        }

        [TestMethod]
        public void FalseIfClassAlreadyHasMocksClass()
        {
            var mocks = new Mocks(MockFactory);

            var variableDeclaration = MockFactory.CreateMock<IVariableDeclaration>();
            mocks.SelectedVariableProviderExpectGetValueOrDefault(variableDeclaration.MockObject);
            mocks.IsVariableNotNullExpectValidate(variableDeclaration.MockObject, true);

            var classDeclaration = MockFactory.CreateMock<IClassDeclaration>();
            mocks.SelectedClassProviderExpectGetValueOrDefault(classDeclaration.MockObject);
            mocks.IsClassHasTestClassAttributeExpectValidate(classDeclaration.MockObject, true);
            mocks.IsClassContainsMocksClassExpectValidate(classDeclaration.MockObject, true);

            Assert.IsFalse(mocks.Sut.Validate());
        }

        [TestMethod]
        public void TrueIfVariableSelectedAndTestClassNotContainsMocksClass()
        {
            var mocks = new Mocks(MockFactory);

            var variableDeclaration = MockFactory.CreateMock<IVariableDeclaration>();
            mocks.SelectedVariableProviderExpectGetValueOrDefault(variableDeclaration.MockObject);
            mocks.IsVariableNotNull.Expects.One.MethodWith(o => o.Validate(variableDeclaration.MockObject)).WillReturn(true);

            var classDeclaration = MockFactory.CreateMock<IClassDeclaration>();
            mocks.SelectedClassProviderExpectGetValueOrDefault(classDeclaration.MockObject);
            mocks.IsClassHasTestClassAttributeExpectValidate(classDeclaration.MockObject, true);
            mocks.IsClassContainsMocksClassExpectValidate(classDeclaration.MockObject, false);

            Assert.IsTrue(mocks.Sut.Validate());
        }

        private sealed class Mocks
        {
            public Mocks(MockFactory mockFactory)
            {
                SelectedClassProvider = mockFactory.CreateMock<INullableValueProvider<IClassDeclaration>>();
                SelectedVariableProvider = mockFactory.CreateMock<INullableValueProvider<IVariableDeclaration>>();
                IsClassHasTestClassAttribute = mockFactory.CreateMock<IValidator<IClassDeclaration>>();
                IsClassContainsMocksClass = mockFactory.CreateMock<IValidator<IClassDeclaration>>();
                IsVariableNotNull = mockFactory.CreateMock<IValidator<IVariableDeclaration>>();

                Sut = new CreateMocksClassContextActionAvailabilityValidator(
                    SelectedClassProvider.MockObject,
                    SelectedVariableProvider.MockObject,
                    IsClassHasTestClassAttribute.MockObject,
                    IsClassContainsMocksClass.MockObject,
                    IsVariableNotNull.MockObject);
            }

            public CreateMocksClassContextActionAvailabilityValidator Sut { get; private set; }

            private Mock<INullableValueProvider<IClassDeclaration>> SelectedClassProvider { get; set; }

            private Mock<INullableValueProvider<IVariableDeclaration>> SelectedVariableProvider { get; set; }

            private Mock<IValidator<IClassDeclaration>> IsClassHasTestClassAttribute { get; set; }

            private Mock<IValidator<IClassDeclaration>> IsClassContainsMocksClass { get; set; }

            public Mock<IValidator<IVariableDeclaration>> IsVariableNotNull { get; private set; }

            public void SelectedClassProviderExpectGetValueOrDefault(IClassDeclaration result)
            {
                SelectedClassProvider.Expects.One
                    .MethodWith(o => o.GetValueOrDefault())
                    .WillReturn(result);
            }

            public void SelectedVariableProviderExpectGetValueOrDefault(IVariableDeclaration result)
            {
                SelectedVariableProvider.Expects.One
                    .MethodWith(o => o.GetValueOrDefault())
                    .WillReturn(result);
            }

            public void IsVariableNotNullExpectValidate(IVariableDeclaration variableDeclaration, bool result)
            {
                IsVariableNotNull.Expects.One
                    .MethodWith(o => o.Validate(variableDeclaration))
                    .WillReturn(result);
            }

            public void IsClassHasTestClassAttributeExpectValidate(IClassDeclaration classDeclaration, bool result)
            {
                IsClassHasTestClassAttribute.Expects.One
                    .MethodWith(o => o.Validate(classDeclaration))
                    .WillReturn(result);
            }

            public void IsClassContainsMocksClassExpectValidate(IClassDeclaration classDeclaration, bool result)
            {
                IsClassContainsMocksClass.Expects.One
                    .MethodWith(o => o.Validate(classDeclaration))
                    .WillReturn(result);
            }
        }
    }
}
