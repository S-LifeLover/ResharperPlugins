using AutoNMock.Validators;
using Common.Tests.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoNMock.Tests.Validators
{
    [TestClass]
    public class IsNotNullTests : TestsWithMocks
    {
        [TestMethod]
        public void TrueIfClassIsNotNull()
        {
            var sut = new IsNotNull<object>();
            Assert.IsTrue(sut.Validate(new object()));
        }

        [TestMethod]
        public void FalseIfClassIsNull()
        {
            var sut = new IsNotNull<object>();
            Assert.IsFalse(sut.Validate(null));
        }
    }
}
