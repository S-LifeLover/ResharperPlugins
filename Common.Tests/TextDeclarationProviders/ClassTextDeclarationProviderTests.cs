using System.Collections.Generic;
using Common.Providers;
using Common.Tests.TestTools;
using Common.TextDeclarationProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Tests.TextDeclarationProviders
{
    [TestClass]
    public class ClassTextDeclarationProviderTests : TestsWithMocks
    {
        [TestMethod]
        public void CreateTest()
        {
            var classMemberTextDeclarationsProvider1 = MockFactory.CreateMock<IProvider<string>>();
            var classMemberTextDeclarationsProvider2 = MockFactory.CreateMock<IProvider<string>>();
            var classMemberTextDeclarationsProviders = new List<IProvider<string>>
                {
                    classMemberTextDeclarationsProvider1.MockObject, 
                    classMemberTextDeclarationsProvider2.MockObject
                };
            var sut = new ClassTextDeclarationProvider(classMemberTextDeclarationsProviders);

            var classMemberTextDeclarations =
                new List<string> { "SomeClassMemberDeclaration1", "SomeClassMemberDeclaration2" };
            classMemberTextDeclarationsProvider1.Expects.One
                .MethodWith(o => o.Get())
                .WillReturn(classMemberTextDeclarations[0]);
            classMemberTextDeclarationsProvider2.Expects.One
                .MethodWith(o => o.Get())
                .WillReturn(classMemberTextDeclarations[1]);
            var actualResult = sut.Get();

            // ToDo_AP: Возможно, стоит сравнивать без учета пробелов.
            Assert.AreEqual(ExpectedResult, actualResult);
        }

        private const string ExpectedResult = @"
            private sealed class Mocks
            {
                SomeClassMemberDeclaration1

                SomeClassMemberDeclaration2
            }";
    }
}
