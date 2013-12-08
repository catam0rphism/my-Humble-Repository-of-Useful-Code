using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLUT;
using System.Linq;

namespace SLUT_Tests
{
    [TestClass]
    public class MathTests
    {
        [TestMethod]
        public void Initialize_Matrix_using_width_height_constructor()
        {
            Matrix m = new Matrix(2, 5);

            CollectionAssert.AreEqual(new double[2, 5], m.GetArray());
        }
        [TestMethod]
        public void Initialize_matrix_using_double_array_comstructor()
        {
            var matrix_data = new double[2, 3] { { 1, 2, 3 }, 
                                                 { 4, 5, 6 } };

            Matrix m = new Matrix(matrix_data);

            CollectionAssert.AreEqual(matrix_data, m.GetArray());
        }

        [TestMethod]
        public void Get_matrix_column_and_row()
        {
            Matrix matrix = new Matrix(new double[2, 3] { { 1, 2, 3 }, 
                                                          { 4, 5, 6 } });
            
            CollectionAssert.AreEqual(new double[] { 4, 5, 6 },  matrix.GetRow(2).ToArray());
            CollectionAssert.AreEqual(new double[] { 2, 5 }, matrix.GetColumn(2).ToArray());
        }

        [TestMethod]
        public void Get_matrix_element_by_index()
        {
            Matrix m = new Matrix(new double[,] { { 1, 2, 3 }, 
                                                  { 4, 5, 6 } });

            Assert.AreEqual(3, m[0, 2]);
            Assert.AreEqual(5, m[1, 1]);
        }

        [TestMethod]
        public void Sum_two_matrix()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 1, 1 }, 
                                                   { 2, 2, 2 } });

            Matrix m2 = new Matrix(new double[,] { { 2, 3, 3 }, 
                                                   { 2, 3, 3 } });

            Matrix m3 = m1 + m2;

            CollectionAssert.AreEqual(new double[,] { { 3, 4, 4 }, { 4, 5, 5 } }, m3.GetArray());

        }

        [TestMethod]
        public void Sum_diferent_size_matrix()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 1, 1 }, 
                                                   { 2, 2, 2 } });

            Matrix m2 = new Matrix(new double[,] { { 4 } });

            try
            {
                Matrix m3 = m1 + m2;
                Assert.Fail();
            }
            catch (ArgumentException e) { }
        }

        [TestMethod]
        public void Subtract_two_matrix()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 1, 1 }, 
                                                   { 2, 2, 2 } }); 
            
            Matrix m2 = new Matrix(new double[,] { { 1, 1, 1 }, 
                                                   { 2, 2, 2 } });

            Matrix m3 = m1 - m2;

            CollectionAssert.AreEqual(new double[,] { { 0, 0, 0 }, { 0, 0, 0 } }, m3.GetArray());
        }

        [TestMethod]
        public void Multiple_two_matrix()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 1, 1 }, 
                                                   { 2, 2, 2 } });

            Matrix m2 = new Matrix(new double[,] { { 1, 3 }, 
                                                   { 4, 5 }, 
                                                   { 2, 2 } });

            Matrix m3 = m1 * m2;

            var res = new double[,] { { 7, 10 }, { 14, 20 } };

            CollectionAssert.AreEqual(res, m3.GetArray());
        }
    }
}
