using Common.Executors;
using Common.ReSharper.Feature.Services.Bulbs;
using Common.Tests.TestTools;
using JetBrains.ProjectModel;
using JetBrains.TextControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace Common.Tests.ReSharper.Feature.Services.Bulbs
{
    // ToDO_AP: подумать над названием. Возможно, в основу класса ляже не Executor
    [TestClass]
    public class SimpleExecutorBasedBulbItemTests : TestsWithMocks
    {
        [TestMethod]
        public void TextCanBeSet()
        {
            var mocks = new Mocks(MockFactory);
            Assert.AreEqual("SomeText", mocks.Sut.Text);
        }

        // ToDo_AP: непонятно как тестировать. В классс BulbItemImpl много жестко связанных зависимостей.
        [Ignore]
        [TestMethod]
        public void ExecutorCanBeSet()
        {
            var mocks = new Mocks(MockFactory);

            var solution = MockFactory.CreateMock<ISolution>();
            var textControl = MockFactory.CreateMock<ITextControl>();
            mocks.Executor.Expects.One.MethodWith(o => o.Execute());
            mocks.Sut.Execute(solution.MockObject, textControl.MockObject);
        }

        private sealed class Mocks
        {
            public Mocks(MockFactory mockFactory)
            {
                Executor = mockFactory.CreateMock<IExecutor>();

                Sut = new SimpleExecutorBasedBulbItem("SomeText", Executor.MockObject);
            }

            public SimpleExecutorBasedBulbItem Sut { get; private set; }

            public Mock<IExecutor> Executor { get; private set; }
        }
    }
}
