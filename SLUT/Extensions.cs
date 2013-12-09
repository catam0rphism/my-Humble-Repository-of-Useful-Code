using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source,Action<T> func)
        {
            foreach (T item in source)
            {
                func(item);
            }
        }

    }

    public static class MatrixExtensions
    {
        #region Higher-order functions
        public static Matrix Map(this Matrix m, Func<double, double> f)
        {
            Matrix m2 = new Matrix(m.Height, m.Width);
            for (int w = 0; w < m.Width; w++)
            {
                for (int h = 0; h < m.Height; h++)
                {
                    m2[h, w] = f(m[h, w]);
                }
            }
            return m2;
        }
        public static Matrix Map2(this Matrix m1,Matrix m2, Func<double, double, double> f)
        {
            if (m1.Height != m2.Height || m1.Width != m2.Width)
                throw new ArgumentException("Матрицы разного размера");

            Matrix m3 = new Matrix(m1.Height,m1.Width);
            for (int w = 0; w < m1.Width; w++)
            {
                for (int h = 0; h < m1.Height; h++)
                {
                    m3[h, w] = f(m1[h, w], m2[h, w]);
                }
            }
            return m3;
        }
        #endregion
    }
}
