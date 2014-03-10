using System.IO;
using System.Numerics;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRUC.Math;

namespace HRUC_Tests
{
    [TestClass]
    public class ComplexPlaneTests
    {
        static ComplexPlane _complexPlane;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            //_complexPlane = new ComplexPlane(0.002, 347, 347*4, new Complex(0, 0));
            _complexPlane = new ComplexPlane(new Complex(0.347 * 2, 0.347 * 8), 347, 347 * 4, Complex.Zero);
        }

        [TestMethod]
        public void Find_max_and_min_value()
        {
            Assert.AreEqual(-0.347, _complexPlane.MinReal, _complexPlane.Delta / 2, "MinReal isn't correct");
            Assert.AreEqual(0.347, _complexPlane.MaxReal, _complexPlane.Delta / 2, "MaxReal isn't correct");

            Assert.AreEqual(-1.388, _complexPlane.MinImag, _complexPlane.Delta / 2, "MinImmag isn't correct");
            Assert.AreEqual(1.388, _complexPlane.MaxImag, _complexPlane.Delta / 2, "MaxImmag isn't correct");
        }

        [TestMethod]
        public void Get_value_using_index()
        {
            Assert.AreEqual(-0.347, _complexPlane[0, 0].Real, _complexPlane.Delta / 2, "Index [0,0] real path isn't correct");
            Assert.AreEqual(1.388, _complexPlane[0, 0].Imaginary, _complexPlane.Delta / 2, "Index [0,0] immag path isn't correct");
        }

        [TestMethod]
        public void Xml_serialization()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ComplexPlane));

            using (FileStream fs = 
                new FileStream("temp", FileMode.Create))
            {
                xs.Serialize(fs, _complexPlane);
            }

            using (FileStream fs =
                new FileStream("temp", FileMode.Open))
            {
                Assert.AreEqual(_complexPlane, xs.Deserialize(fs));
                //Assert.IsTrue(complexPlane==(ComplexPlane)xs.Deserialize(fs));
            }
            File.Delete("temp");
        }

        [TestMethod]
        public void Clone_complex_plane()
        {
            ComplexPlane test = _complexPlane.Clone() as ComplexPlane;

            Assert.IsFalse(ReferenceEquals(test, _complexPlane));
            Assert.AreEqual(_complexPlane,test);

        }
    }
}