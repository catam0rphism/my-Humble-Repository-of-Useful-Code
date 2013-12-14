using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLUT.Math;
using System.Linq;

namespace SLUT_Tests
{
    [TestClass]
    public class MathTests
    {
        #region constructor tests
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
        #endregion
        #region row and column tests
        [TestMethod]
        public void Get_matrix_column_and_row()
        {
            Matrix matrix = new Matrix(new double[2, 3] { { 1, 2, 3 }, 
                                                          { 4, 5, 6 } });
            
            CollectionAssert.AreEqual(new double[] { 4, 5, 6 },  matrix.GetRow(2).ToArray());
            CollectionAssert.AreEqual(new double[] { 2, 5 }, matrix.GetColumn(2).ToArray());
        }

        [TestMethod]
        public void Swich_matrix_columns_and_rows()
        {
            Matrix matrix = new Matrix(new double[2, 3] { { 1, 2, 3 }, 
                                                          { 4, 5, 6 } });

            matrix.SwitchColumn(1, 2);

            CollectionAssert.AreEqual(new double[,] { { 2, 1, 3 }, { 5, 4, 6 } }, matrix.GetArray());

            matrix.SwitchRow(1, 2);

            CollectionAssert.AreEqual(new double[,] { { 5, 4, 6 }, { 2, 1, 3 } }, matrix.GetArray());
        }

        [TestMethod]
        public void Multiplicate_columns_and_rows()
        {
            Matrix matrix = new Matrix(new double[2, 3] { { 1, 2, 3 }, 
                                                          { 4, 5, 6 } });

            matrix = matrix.MultiplicateColumn(1, 2);

            CollectionAssert.AreEqual(new double[,] { { 2, 2, 3 }, { 8, 5, 6 } },matrix.GetArray());

            matrix = matrix.MultiplicateRow(2, 3);

            CollectionAssert.AreEqual(new double[,] { { 2, 2, 3 }, { 24, 15, 18 } }, matrix.GetArray());
        }

        [TestMethod]
        public void Add_column_and_row_test()
        {
            Matrix matrix = new Matrix(new double[2, 3] { { 1, 2, 3 }, 
                                                          { 4, 5, 6 } });

            matrix = matrix.AddRow(1, 2);

            CollectionAssert.AreEqual(new double[,] { { 5, 7, 9 }, { 4, 5, 6 } }, matrix.GetArray(),"Error in AddRow()");

            matrix = matrix.AddColumn(2, 3);

            CollectionAssert.AreEqual(new double[,] { { 5, 16, 9 }, { 4, 11, 6 } }, matrix.GetArray());
        }
        
        #endregion
        #region operators and indexator tests
        [TestMethod]
        public void Get_matrix_element_by_index()
        {
            Matrix m = new Matrix(new double[,] { { 1, 2, 3 }, 
                                                  { 4, 5, 6 } });

            Assert.AreEqual(3, m[1, 3]);
            Assert.AreEqual(5, m[2, 2]);

            try
            {
                var n = m[0, 0];
                Assert.Fail();
            }
            catch (IndexOutOfRangeException ex)
            { }
        }

        [TestMethod]
        public void Sum_two_matrix()
        {
            Matrix m1 = new Matrix(new double[,] { { 1, 1, 1 }, 
                                                   { 2, 2, 2 } });

            Matrix m2 = new Matrix(new double[,] { { 2, 3, 3 }, 
                                                   { 2, 3, 3 } });

            Matrix m3 = m1 + m2;

            CollectionAssert.AreEqual(new double[,] { { 3, 4, 4 }, { 4, 5, 5 } }, (double[,])m3);

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

        [TestMethod]
        public void Multiple_matrix_and_number()
        {
            Matrix m = new Matrix(new double[,] { { 5, 4, 3 }, 
                                                  { 4, 3, 5 } });

            m *= 10;

            CollectionAssert.AreEqual(new double[,] { { 50, 40, 30 }, { 40, 30, 50 } }, m.GetArray());

        }
        #endregion
        [TestMethod]
        public void Transform_to_tringular_matrix()
        {
            Matrix m = new Matrix(new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } });

            m.ToTringularForm();

            CollectionAssert.AreEqual(new double[,] { { 1, 2, 3 }, 
                                                      { 0, -3, -6 }, 
                                                      { 0, 0, 0 } }, m.GetArray());
        }
        [TestMethod]
        public void Get_tringular_matrix_determinant()
        {
            Matrix m = new Matrix(new double[,] 
            { { -1, 2, -3 }, 
              { 4, -5, 6 }, 
              { -7, 8, 9 } });

            var res = m.Determinant();

            Assert.AreEqual(res, -54);
        }

        [TestMethod]
        public void Transposition_Matrix()
        {
            Matrix m = new Matrix(
                new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });

            m = m.Transposition();

            CollectionAssert.AreEqual(new double[,] { { 1, 4 }, { 2, 5 }, { 3, 6 } }, m.GetArray());
        }
    }
}
