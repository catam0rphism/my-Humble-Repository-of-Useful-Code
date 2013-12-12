using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> func)
        {
            foreach (T item in source)
            {
                func(item);
            }
        }

    }

    namespace Math
    {
        public static class MatrixExtensions
        {
            #region Higher-order functions
            /// <summary>
            /// Проецирует каждый элемент на новую матрицу
            /// </summary>
            /// <param name="m">Исходная матрица</param>
            /// <param name="f">Функция преобразования</param>
            /// <returns></returns>
            public static Matrix Map(this Matrix m, Func<double, double> f)
            {
                Matrix m2 = new Matrix(m.RowCount, m.ColumnCount);
                for (int w = 0; w < m.ColumnCount; w++)
                {
                    for (int h = 0; h < m.RowCount; h++)
                    {
                        m2[h, w] = f(m[h, w]);
                    }
                }
                return m2;
            }
            /// <summary>
            /// Проецирует каждый элемент на новую матрицу
            /// </summary>
            /// <param name="m">Исходная матрица</param>
            /// <param name="f">Функция преобразования, использующая индексы элемента</param>
            /// <returns></returns>
            public static Matrix Map(this Matrix m, Func<int, int, double, double> f)
            {
                /// f: row_num column_num value -> f(...)
                m = (Matrix)m.Clone();
                for (int i = 0; i < m.ColumnCount; i++)
                {
                    for (int j = 0; j < m.RowCount; j++)
                    {
                        m[j, i] = f(j, i, m[j, i]);
                    }
                }
                return m;
            }
            /// <summary>
            /// Создает носую матрицу на основе двух других
            /// последовательным применением функции
            /// </summary>
            /// <param name="m1">Первая матрица</param>
            /// <param name="m2">Вторая матрица</param>
            /// <param name="f">Функция преобразования</param>
            /// <returns></returns>
            public static Matrix Map2(this Matrix m1, Matrix m2, Func<double, double, double> f)
            {
                if (m1.RowCount != m2.RowCount || m1.ColumnCount != m2.ColumnCount)
                    throw new ArgumentException("Матрицы разного размера");

                Matrix m3 = new Matrix(m1.RowCount, m1.ColumnCount);
                for (int w = 0; w < m1.ColumnCount; w++)
                {
                    for (int h = 0; h < m1.RowCount; h++)
                    {
                        m3[h, w] = f(m1[h, w], m2[h, w]);
                    }
                }
                return m3;
            }
            /// <summary>
            /// Применяет к каждуму элементу одного столбца функцию
            /// </summary>
            /// <param name="m">Исходная матрица</param>
            /// <param name="row_num">Номер изменяемого столбца</param>
            /// <param name="f">Функция, применяемая для отображения элемента</param>
            /// <returns></returns>
            public static Matrix ColumnMap(this Matrix m, int column_num, Func<double, double> f)
            {
                m = (Matrix)m.Clone();

                column_num--;
                for (int row = 0; row < m.RowCount; row++)
                {
                    m[row, column_num] = f(m[row, column_num]);
                }
                return m;
            }
            /// <summary>
            /// Применяет к каждуму элементу одной строки функцию
            /// </summary>
            /// <param name="m">Исходная матрица</param>
            /// <param name="row_num">Номер изменяемой строки</param>
            /// <param name="f">Функция, применяемая для отображения элемента</param>
            /// <returns></returns>
            public static Matrix RowMap(this Matrix m, int row_num, Func<double, double> f)
            {
                m = (Matrix)m.Clone();
                row_num--;
                for (int w = 0; w < m.ColumnCount; w++)
                {
                    m[row_num, w] = f(m[row_num, w]);
                }
                return m;
            }
            #endregion
        }
    }
}
