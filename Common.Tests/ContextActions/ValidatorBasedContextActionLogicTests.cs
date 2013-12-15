using Common.ContextActions;
using Common.Tests.TestTools;
using Common.Validators;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace Common.Tests.ContextActions
{
    [TestClass]
    public class ValidatorBasedContextActionLogicTests : TestsWithMocks
    {
        [TestMethod]
        public void ActionIsAvailable()
        {
            var mocks = new Mocks(MockFactory);
            var userDataHolder = MockFactory.CreateMock<IUserDataHolder>();
            mocks.Validator.Expects.One.MethodWith(o => o.Validate()).WillReturn(true);
            Assert.IsTrue(mocks.Sut.IsAvailable(userDataHolder.MockObject));
        }

        [TestMethod]
        public void ActionIsNotAvailable()
        {
            var mocks = new Mocks(MockFactory);
            var userDataHolder = MockFactory.CreateMock<IUserDataHolder>();
            mocks.Validator.Expects.One.MethodWith(o => o.Validate()).WillReturn(false);
            Assert.IsFalse(mocks.Sut.IsAvailable(userDataHolder.MockObject));
        }

        [TestMethod]
        public void ActionReturnsBulbItem()
        {
            var mocks = new Mocks(MockFactory);
            Assert.AreEqual(1, mocks.Sut.Items.Length);
            Assert.AreSame(mocks.BulbItem.MockObject, mocks.Sut.Items[0]);
        }

        private sealed class Mocks
        {
            public Mocks(MockFactory mockFactory)
            {
                Validator = mockFactory.CreateMock<IValidator>();
                BulbItem = mockFactory.CreateMock<IBulbItem>();

                Sut = new ValidatorBasedContextActionLogic(Validator.MockObject, BulbItem.MockObject);
            }

            public ValidatorBasedContextActionLogic Sut { get; private set; }

            public Mock<IValidator> Validator { get; private set; }

            public Mock<IBulbItem> BulbItem { get; private set; }
        }
    }
}
