using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace Common.Tests.TestTools
{
    [TestClass]
    public abstract class TestsWithMocks : IDisposable
    {
        [CLSCompliant(false)]
        public MockFactory MockFactory
        {
            get { return _mockFactory; }
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
            MockFactory.VerifyAllExpectationsHaveBeenMet();
        }

        private readonly MockFactory _mockFactory = new MockFactory();

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mockFactory.Dispose();
            }
        }
    }
}
