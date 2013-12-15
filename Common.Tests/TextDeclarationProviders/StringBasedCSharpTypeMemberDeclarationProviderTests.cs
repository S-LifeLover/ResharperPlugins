using Common.Providers;
using Common.ReSharper.Psi.CSharp;
using Common.Tests.TestTools;
using Common.TextDeclarationProviders;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace Common.Tests.TextDeclarationProviders
{
    [TestClass]
    public class StringBasedCSharpTypeMemberDeclarationFactoryTests : TestsWithMocks
    {
        [TestMethod]
        public void CreateMocksClass()
        {
            var mocks = new Mocks(MockFactory);

            var expectedResult = MockFactory.CreateMock<ICSharpTypeMemberDeclaration>().MockObject;
            mocks.TypeMemberTextDeclarationProvider.Expects.One
                .MethodWith(o => o.Get())
                .WillReturn("SomeDeclaration");
            mocks.CSharpElementFactory.Expects.One
                .MethodWith(o => o.CreateTypeMemberDeclaration("SomeDeclaration"))
                .WillReturn(expectedResult);
            var actualResult = mocks.Sut.Create();
            Assert.AreSame(expectedResult, actualResult);
        }

        private sealed class Mocks
        {
            public Mocks(MockFactory mockFactory)
            {
                CSharpElementFactory = mockFactory.CreateMock<ICSharpElementFactory>();
                TypeMemberTextDeclarationProvider = mockFactory.CreateMock<IProvider<string>>();

                Sut = new StringBasedCSharpTypeMemberDeclarationFactory(
                    CSharpElementFactory.MockObject, TypeMemberTextDeclarationProvider.MockObject);
            }

            public StringBasedCSharpTypeMemberDeclarationFactory Sut { get; private set; }

            public Mock<ICSharpElementFactory> CSharpElementFactory { get; private set; }

            public Mock<IProvider<string>> TypeMemberTextDeclarationProvider { get; private set; }
        }
    }
}
