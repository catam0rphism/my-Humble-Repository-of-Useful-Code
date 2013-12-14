using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLUT;
using SLUT.Math;

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

        [TestMethod]
        public void Map_Func_Test()
        {
            Matrix m = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            Func<double, double> f = x => x * x + 25;

            m = m.Map(f);

            CollectionAssert.AreEqual(new double[,] { { 26, 29, 34 }, { 41, 50, 61 } },m.GetArray());
        }
    }
}
