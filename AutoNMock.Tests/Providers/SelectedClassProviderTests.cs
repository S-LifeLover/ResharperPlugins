using AutoNMock.Providers;
using Common.Tests.TestTools;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoNMock.Tests.Providers
{
    // ToDO_AP: устранить дублирование
    [TestClass]
    public class SelectedClassProviderTests : TestsWithMocks
    {
        [TestMethod]
        public void ReturnSelectedClassIfExists()
        {
            var contextActionDataProvider = MockFactory.CreateMock<ICSharpContextActionDataProvider>();
            var sut = new SelectedClassProvider(contextActionDataProvider.MockObject);

            var expectedClassDeclaration = MockFactory.CreateMock<IClassDeclaration>();
            contextActionDataProvider.Expects.One
                .MethodWith(o => o.GetSelectedElement<IClassDeclaration>(false, true))
                .WillReturn(expectedClassDeclaration.MockObject);
            Assert.AreSame(expectedClassDeclaration.MockObject, sut.GetValueOrDefault());
        }

        [TestMethod]
        public void ReturnNullIfSelectedClassNotExists()
        {
            var contextActionDataProvider = MockFactory.CreateMock<ICSharpContextActionDataProvider>();
            var sut = new SelectedClassProvider(contextActionDataProvider.MockObject);

            contextActionDataProvider.Expects.One
                .MethodWith(o => o.GetSelectedElement<IClassDeclaration>(false, true))
                .WillReturn(null);
            Assert.IsNull(sut.GetValueOrDefault());
        }
    }
}
