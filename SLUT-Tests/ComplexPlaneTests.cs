using System;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLUT.Math;

namespace FractalVisualizerUnitTests
{
    [TestClass]
    public class ComplexPlaneTests
    {
        static ComplexPlane cp = null;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            cp = new ComplexPlane(0.002, 347, 347*4, new System.Numerics.Complex(0, 0));
        }

        [TestMethod]
        public void Find_max_and_min_value()
        {
            Assert.AreEqual(-0.347, cp.MinReal, cp.Delta / 2, "MinReal isn't correct");
            Assert.AreEqual(0.347, cp.MaxReal, cp.Delta / 2, "MaxReal isn't correct");

            Assert.AreEqual(-1.388, cp.MinImag, cp.Delta / 2, "MinImmag isn't correct");
            Assert.AreEqual(1.388, cp.MaxImag, cp.Delta / 2, "MaxImmag isn't correct");
        }

        [TestMethod]
        public void Get_value_using_index()
        {
            Assert.AreEqual(-0.347, cp[0, 0].Real, cp.Delta / 2, "Index [0,0] real path isn't correct");
            Assert.AreEqual(1.388, cp[0, 0].Imaginary, cp.Delta / 2, "Index [0,0] immag path isn't correct");
        }

        [TestMethod]
        public void Xml_serialization()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ComplexPlane));

            using (System.IO.FileStream fs = 
                new System.IO.FileStream("temp", System.IO.FileMode.Create))
            {
                xs.Serialize(fs, cp);
            }

            using (System.IO.FileStream fs =
                new System.IO.FileStream("temp", System.IO.FileMode.Open))
            {
                Assert.AreEqual(cp, xs.Deserialize(fs));
                //Assert.IsTrue(cp==(ComplexPlane)xs.Deserialize(fs));
            }
            System.IO.File.Delete("temp");
        }

        [TestMethod]
        public void Clone_complex_plane()
        {
            ComplexPlane test = cp.Clone() as ComplexPlane;

            Assert.IsFalse(object.ReferenceEquals(test, cp));
            Assert.AreEqual(cp,test);

        }
    }
}