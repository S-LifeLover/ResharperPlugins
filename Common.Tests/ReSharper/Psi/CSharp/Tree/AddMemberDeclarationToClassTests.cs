using Common.Factories;
using Common.Providers;
using Common.ReSharper.Psi.CSharp.Tree;
using Common.Tests.TestTools;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace Common.Tests.ReSharper.Psi.CSharp.Tree
{
    [TestClass]
    public class AddMemberDeclarationToClassTests : TestsWithMocks
    {
        [TestMethod]
        public void AddMocksClassToSelectedClass()
        {
            var mocks = new Mocks(MockFactory);

            var mocksClass = MockFactory.CreateMock<IClassDeclaration>();
            mocks.ClassMemberDeclarationFactory.Expects.One.MethodWith(o => o.Create()).WillReturn(mocksClass.MockObject);

            var selectedClass = MockFactory.CreateMock<IClassDeclaration>();
            mocks.ClassProvider.Expects.One
                .MethodWith(o => o.GetValueOrDefault())
                .WillReturn(selectedClass.MockObject);

            selectedClass.Expects.One
                .MethodWith(o => o.AddClassMemberDeclaration(mocksClass.MockObject))
                .WillReturn(mocksClass.MockObject);
            mocks.Sut.Execute();
        }

        [TestMethod]
        public void NotCreateMocksClassIfNoSelectedClass()
        {
            var mocks = new Mocks(MockFactory);

            var mocksClass = MockFactory.CreateMock<IClassDeclaration>();
            mocks.ClassMemberDeclarationFactory.Expects.One.MethodWith(o => o.Create()).WillReturn(mocksClass.MockObject);

            mocks.ClassProvider.Expects.One
                .MethodWith(o => o.GetValueOrDefault())
                .WillReturn(null);

            mocks.Sut.Execute();
        }

        private sealed class Mocks
        {
            public Mocks(MockFactory mockFactory)
            {
                ClassProvider = mockFactory.CreateMock<INullableValueProvider<IClassDeclaration>>();
                ClassMemberDeclarationFactory = mockFactory.CreateMock<IFactory<IClassDeclaration>>();

                Sut = new AddMemberDeclarationToClass(
                    ClassProvider.MockObject, ClassMemberDeclarationFactory.MockObject);
            }

            public AddMemberDeclarationToClass Sut { get; private set; }

            public Mock<INullableValueProvider<IClassDeclaration>> ClassProvider { get; private set; }

            public Mock<IFactory<IClassDeclaration>> ClassMemberDeclarationFactory { get; private set; }
        }
    }
}
