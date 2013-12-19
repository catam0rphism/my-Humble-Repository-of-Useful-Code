using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRUC
{
    public static class Extensions
    {
        // Обертка над циклом foreach
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> func)
        {
            foreach (T item in source)
            {
                func(item);
            }
        }

        public static U Null<T, U>(this T @object, Func<T, U> func)
            where U : class
            where T : class
        {
            if (@object != null)
            {
                return func(@object);
            }
            else
            {
                return null;
            }
        }

    }

    namespace Math
    {
        public static class MatrixExtensions
        {
            #region map functions
            /// <summary>
            /// Проецирует каждый элемент на новую матрицу
            /// </summary>
            /// <param name="m">Исходная матрица</param>
            /// <param name="f">Функция преобразования</param>
            public static Matrix Map(this Matrix m, Func<double, double> f)
            {
                Matrix m2 = new Matrix(m.RowCount, m.ColumnCount);
                for (int w = 1; w <= m.ColumnCount; w++)
                {
                    for (int h = 1; h <= m.RowCount; h++)
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
                for (int i = 1; i <= m.ColumnCount; i++)
                {
                    for (int j = 1; j <= m.RowCount; j++)
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
                for (int w = 1; w <= m1.ColumnCount; w++)
                {
                    for (int h = 1; h <= m1.RowCount; h++)
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

                for (int row = 1; row <= m.RowCount; row++)
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
                for (int w = 1; w <= m.ColumnCount; w++)
                {
                    m[row_num, w] = f(m[row_num, w]);
                }
                return m;
            }
            /// <summary>
            /// Выполняет свертку матрицы в порядке слева направо, сверху вниз
            /// </summary>
            /// <param name="m">Исходная матрица</param>
            /// <param name="seed">Начальное значение аккумулятора</param>
            /// <param name="f">Функция свертки</param>
            #endregion
            // свертка во порядке Строка (L -> R) -> Столбец (U -> D)
            public static double FoldRC(this Matrix m,double seed, Func<double, double, double> f)
            {
                double acc = seed;
                for (int column_number = 1; column_number <= m.ColumnCount; column_number++)
                {
                    for (int row_num = 1; row_num <= m.RowCount; row_num++)
                    {
                        acc = f(acc, m[row_num, column_number]);
                    }
                }
                return acc;
            }
            public static T FoldRC<T>(this Matrix m, T seed, Func<T, double, T> f)
            {
                T acc = seed;
                for (int column_number = 1; column_number <= m.ColumnCount; column_number++)
                {
                    for (int row_num = 1; row_num <= m.RowCount; row_num++)
                    {
                        acc = f(acc, m[row_num, column_number]);
                    }
                }
                return acc;
            }

            /// <summary>
            /// Выполняет процедуру для каждого элемента матрицы
            /// </summary>
            /// <param name="m">Исходная матрица</param>
            /// <param name="f">Процедура, выполняемая для каждого элемента</param>
            public static Matrix Iter(this Matrix m, Action<int, int, double> f)
            {
                for (int i = 1; i <= m.ColumnCount; i++)
                {
                    for (int j = 1; j <= m.RowCount; j++)
                    {
                        f(j, i, m[j, i]);
                    }
                }
                return m;
            }
            #region max/min
            /// <summary>
            /// Выполняет поиск максимального элемента матрицы
            /// </summary>
            /// <param name="m">Матрица</param>
            static double Max(this Matrix m)
            {
                return m.FoldRC(double.MinValue, System.Math.Max);
            }
            /// <summary>
            /// Выполняет поиск минимального элемента матрицы
            /// </summary>
            /// <param name="m">Матрица</param>
            static double Min(this Matrix m)
            {
                return m.FoldRC(double.MaxValue, System.Math.Min);
            }
            #endregion

            /// <summary>
            /// Выполняет поиск строки и столбца первого вхождения элемента
            /// </summary>
            /// <param name="m">Матрица</param>
            /// <param name="el">Искомый элемент</param>
            /// <param name="row_number">[out] Номер строки</param>
            /// <param name="column_number">[out] Номер столбца</param>
            /// <returns></returns>
            static Matrix FindIndex(Matrix m, double el,out int row_number,out int column_number)
            {
                for (int row_num = 1; row_num <= m.RowCount; row_num++)
                {
                    for (int column_num = 1; column_num <= m.ColumnCount; column_num++)
                    {
                        if (m[row_num, column_num] == el)
                        {
                            row_number = row_num;
                            column_number = column_num;

                            return m;
                        }
                    }
                }
                throw new ArgumentOutOfRangeException("Матрица не содержит элемента");
            }
        }
    }
}
