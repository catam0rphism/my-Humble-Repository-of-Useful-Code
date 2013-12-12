using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT.Math
{
    public class Matrix
        :ICloneable
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
        public double this[int row_index,int column_index]
        {
            get {return matrix[row_index,column_index];}
            set {matrix[row_index,column_index] = value;}
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
            column--; // n - не индекс а номер ( n=1 -> n=0 )
            for (int i = 0; i < RowCount; i++)
            {
                yield return matrix[i, column];
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
            row -= 1; // n - не индекс а номер ( n=1 -> n=0 )
            for (int i = 0; i < ColumnCount; i++)
            {
                yield return matrix[row, i];
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
            row1--; row2--;
            for (int i = 0; i < ColumnCount; i++)
            {
                double swap_var = matrix[row1, i];
                matrix[row1, i] = matrix[row2, i];
                matrix[row2, i] = swap_var;
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
            column1--; column2--;
            for (int i = 0; i < RowCount; i++)
            {
                double swap_var = matrix[i, column1];
                matrix[i, column1] = matrix[i, column2];
                matrix[i, column2] = swap_var;
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
            row2--; row1--;
            for (int i = 0; i < RowCount; i++)
            {
                matrix[row1, i] += matrix[row2, i] * k;
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
            // it's so bad!
            column1--; column2--;

            for (int i = 0; i < ColumnCount; i++)
            {
                matrix[i, column1] += matrix[i, column2] * k;
            }
            return this;
        }
        /// <summary>
        /// Приводит матрицу к треугольному виду
        /// </summary>
        public Matrix ToTringularForm()
        {
            for (int k = 1; k < RowCount; k++)
            {
                for (int i = k+1; i <= RowCount; i++)
                {
                    // хз что делать при делении на 0
                    if (this[k - 1, k - 1] == 0) throw new DivideByZeroException();

                    AddRow(i, k, -this[i - 1, k-1] / this[k-1, k-1]);
                }
            }

            return this;
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

            for (int i = 0; i < res_h; i++)
            {
                for (int j = 0; j < res_w; j++)
                {
                    var row = m1.GetRow(i+1); // i+1 - тк GetRow() берет номер! а не индекс
                    var column = m2.GetColumn(j+1);
                    
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
        /// <returns></returns>
        public double Determinant()
        {
            Matrix m = (Matrix)Clone();
            // TODO: исправить
            m.ToTringularForm();
            double res = 1;
            for (int i = 0; i < ColumnCount; i++)
            {
                res*=this[i,i];
            }
            return res;
        }

        /// <summary>
        /// Создает копию текущего экземпляра
        /// </summary>
        public object Clone()
        {
            double[,] m = (double[,])GetArray().Clone();
            return new Matrix(m);
        }
    }
}
