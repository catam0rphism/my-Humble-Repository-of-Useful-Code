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
            Matrix m = new Matrix(4, 4);

            CollectionAssert.AreEqual(new double[4, 4], m.GetArray());
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
        }
    }
}
