using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT.Math
{
    public class Matrix
        :ICloneable, IEquatable<Matrix>
    {
        #region Constructors
        /// <summary>
        /// Создает матрицу определенного размера
        /// </summary>
        /// <param name="width">ширина матрицы</param>
        /// <param name="height">высота матрицы</param>
        public Matrix(int height,int width)
        {
            matrix = new double[height,width];            
        }

        /// <summary>
        /// Создает матрицу на основе двумерного массива
        /// </summary>
        /// <param name="matrix_data">Массив элементов матрицы</param>
        public Matrix(double[,] matrix_data)
        {
            matrix = matrix_data;
        }
        #endregion

        /// <summary>
        /// Преобразует матрицу к виду двумерного массива
        /// </summary>
        /// <returns>Двумерный массив эквавалентый этой матрице</returns>
        public double[,] GetArray()
        { 
            return matrix;
        }
        public static implicit operator double[,](Matrix m)
        {
            return m.GetArray();
        }
        /// <summary>
        /// Задает или возвращает элемент матрицы
        /// </summary>
        /// <param name="row_index">Индекс строки</param>
        /// <param name="column_index">Индекс столбца</param>
        /// <returns>Элемент матрицы, занимающий данную позицию</returns>
        public double this[int row_number, int column_number]
        {
            get { return matrix[row_number - 1, column_number - 1]; }
            set { matrix[row_number - 1, column_number - 1] = value; }
        }

        /// <summary>
        /// Возвращает число столбцов матрицы
        /// </summary>
        public int ColumnCount
        {
            get { return matrix.GetLength(1); }
        }
        /// <summary>
        /// Возвращает число строк матрицы
        /// </summary>
        public int RowCount
        {
            get { return matrix.GetLength(0); }
        }

        /// <summary>
        /// Формирует список из элементов колонки матрицы
        /// </summary>
        /// <param name="column">Номер колонки</param>
        /// <returns>Список элементов строки</returns>
        public IEnumerable<double> GetColumn(int column)
        {
            for (int row = 1; row <= RowCount; row++)
            {
                yield return this[row, column];
            }
            yield break;
        }
        /// <summary>
        /// Формирует список элементов строки матрицы
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <returns>Список элементов строки</returns>
        public IEnumerable<double> GetRow(int row)
        {
            for (int column = 1; column <= ColumnCount; column++)
            {
                yield return this[row, column];
            }
            yield break;
        }

        /// <summary>
        /// Переставляет 2 строки матрицы
        /// </summary>
        /// <param name="row1">Номер первой строки</param>
        /// <param name="row2">Номер второй строки</param>
        public Matrix SwitchRow(int row1, int row2)
        {
            for (int i = 1; i <= ColumnCount; i++)
            {
                double swap_var = this[row1, i];
                this[row1, i] = this[row2, i];
                this[row2, i] = swap_var;
            }
            return this;
        }
        /// <summary>
        /// Переставляет 2 столбца матрицы
        /// </summary>
        /// <param name="column1">Номер первого столбца</param>
        /// <param name="column2">Номер второго столбца</param>
        public Matrix SwitchColumn(int column1,int column2)
        {
            for (int i = 1; i <= RowCount; i++)
            {
                double swap_var = this[i, column1];
                this[i, column1] = this[i, column2];
                this[i, column2] = swap_var;
            }
            return this;
        }
        /// <summary>
        /// Умножает на константу элементы одной из строк матрицы
        /// </summary>
        /// <param name="row">Номер строки</param>
        /// <param name="k">Константа</param>
        /// <returns></returns>
        public Matrix MultiplicateRow(int row, double k)
        {
            return this.RowMap(row, a => a * k);
        }
        /// <summary>
        /// Умножает на константу элементы одного из столбцов
        /// </summary>
        /// <param name="column"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public Matrix MultiplicateColumn(int column, double k)
        {
            return this.ColumnMap(column, a => a * k);
        }

        /// <summary>
        /// Складывает 2 строки матрицы
        /// </summary>
        /// <param name="row1">Строка, к которой плюсуют другую</param>
        /// <param name="row2">Прибавляемая строка</param>
        /// <param name="k">Коэфициент, на который умножается вторая строка</param>
        public Matrix AddRow(int row1, int row2,double k = 1)
        {
            for (int column = 1; column <= ColumnCount; column++)
            {
                this[row1, column] += this[row2, column] * k;
            }
            return this;
        }
        /// <summary>
        /// Складывает 2 столбца матрицы
        /// </summary>
        /// <param name="column1">Столбец к которому прибарляют</param>
        /// <param name="column2">Прибавляемый столбец</param>
        /// <param name="k">Коэфициент, на который умножается вторая строка</param>
        public Matrix AddColumn(int column1, int column2,double k = 1)
        {
            for (int row = 1; row <= RowCount; row++)
            {
                this[row, column1] += this[row, column2] * k;
            }
            return this;
        }
        /// <summary>
        /// Приводит матрицу к треугольному виду
        /// </summary>
        public Matrix ToTringularForm()
        {
            /// 1 2 3
            /// 4 5 6
            /// 7 8 9

            int curr_row = 1;
            int curr_column = 1;

            while (curr_row <= RowCount || curr_column <= ColumnCount)
            {
                for (int row_num = curr_row + 1; row_num <= RowCount; row_num++)
                {
                    // Вопрос деления на 0 решен! пропускаем обнуление строки
                    if (this[curr_row, curr_column] == 0) break;

                    // Обнуление строки
                    AddRow(row_num, curr_row, -this[row_num, curr_column] / this[curr_row, curr_column]);
                }
                curr_row++;
                curr_column++;
            }

            return this;
        }

        public  Matrix Transposition()
        {
            Matrix m = new Matrix(ColumnCount, RowCount);

            this.Map((row, column, value) => m[column, row] = value);

            return m;
        }

        private double[,] matrix;

        #region operators
        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            Matrix m = matrix1.Map2(matrix2, (a, b) => a + b);
            return m;
        }
        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            Matrix m = matrix1.Map2(matrix2, (a, b) => a - b);
            return m;
        }
        public static Matrix operator *(Matrix m, double k)
        {
            Matrix res = m.Map(a => a * k);
            return res;
            
        }
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.RowCount != m2.ColumnCount)
                throw new ArgumentException("Форма матриц не согласована");
            
            int res_w, res_h;
            res_w = m2.ColumnCount;
            res_h = m1.RowCount;

            Matrix res = new Matrix(res_h,res_w);

            for (int i = 1; i <= res_h; i++)
            {
                for (int j = 1; j <= res_w; j++)
                {
                    var row = m1.GetRow(i);
                    var column = m2.GetColumn(j);
                    
                    // Складываем произведения соответствующих элементов
                    res[i,j] = Enumerable.Zip(row, column, (a, b) => a * b).Sum();
                }
            }

            return res;
        }
        #endregion

        /// <summary>
        /// Расчет определителя. В ходе расчета матрица приводится к треугольной!
        /// </summary>
        /// UPD: Уже нет. Исползуется копия!
        public double Determinant()
        {
            Matrix m = (Matrix)Clone();
            m = m.ToTringularForm();

            double res = 1;
            for (int i = 1; i <= ColumnCount; i++)
            {
                res*=m[i,i];
            }
            return res;
        }

        #region interfaces
        /// <summary>
        /// Создает копию текущего экземпляра
        /// </summary>
        public object Clone()
        {
            double[,] m = (double[,])GetArray().Clone();
            return new Matrix(m);
        }
        /// <summary>
        /// Сравнивает 2 матрицы
        /// </summary>
        public bool Equals(Matrix other)
        {
            bool IsEquals = true;

            other.Iter((row, column, value) => IsEquals &= this[row, column] == value);

            return IsEquals;
        }
        #endregion
    }
}
