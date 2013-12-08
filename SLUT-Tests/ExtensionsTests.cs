using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLUT;

namespace SLUT_Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void ForEachTest()
        {
            var seq = new byte[] { 0x13, 0xA1, 0xFF };

            int acc = 0;
            seq.ForEach(n => acc+=n);

            Assert.AreEqual(0x13+ 0xA1+ 0xFF, acc);

        }
    }
}
