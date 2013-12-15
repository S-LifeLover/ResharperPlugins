using AutoNMock.Providers;
using Common.Tests.TestTools;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoNMock.Tests.Providers
{
    // ToDO_AP: устранить дублирование
    [TestClass]
    public class SelectedVariableProviderTests : TestsWithMocks
    {
        [TestMethod]
        public void ReturnSelectedVariableIfExists()
        {
            var contextActionDataProvider = MockFactory.CreateMock<ICSharpContextActionDataProvider>();
            var sut = new SelectedVariableProvider(contextActionDataProvider.MockObject);

            var expectedVariableDeclaration = MockFactory.CreateMock<IVariableDeclaration>();
            contextActionDataProvider.Expects.One
                .MethodWith(o => o.GetSelectedElement<IVariableDeclaration>(false, true))
                .WillReturn(expectedVariableDeclaration.MockObject);
            Assert.AreSame(expectedVariableDeclaration.MockObject, sut.GetValueOrDefault());
        }

        [TestMethod]
        public void ReturnNullIfSelectedClassNotExists()
        {
            var contextActionDataProvider = MockFactory.CreateMock<ICSharpContextActionDataProvider>();
            var sut = new SelectedVariableProvider(contextActionDataProvider.MockObject);

            contextActionDataProvider.Expects.One
                .MethodWith(o => o.GetSelectedElement<IVariableDeclaration>(false, true))
                .WillReturn(null);
            Assert.IsNull(sut.GetValueOrDefault());
        }
    }
}
