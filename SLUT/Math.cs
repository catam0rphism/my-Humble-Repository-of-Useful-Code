using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT
{
    public class Matrix
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
        // TODO: Прямое и непрямое(?) приведение к double[,]
        
        /// 1 2 3
        /// 4 5 6
        /// element [1,0] == 4
        /// element [0,2] == 3
        /// TODO: Make test )
        public double this[int row_index,int column_index]
        {
            get {return matrix[row_index,column_index];}
            set {matrix[row_index,column_index] = value;}
        }
        // Внимание! не путать столбцы (h) и строки (w)

        /// <summary>
        /// Ширина матрицы (длина каждой строки)
        /// </summary>
        public int Width
        {
            get { return matrix.GetLength(1); }
        }

        /// <summary>
        /// Высота матрицы (длина каждого столбца)
        /// </summary>
        public int Height
        {
            get { return matrix.GetLength(0); }
        }

        public IEnumerable<double> GetColumn(int column)
        {
            column -= 1; // n - не индекс а номер ( n=1 -> n=0 )
            for (int i = 0; i < Height; i++)
            {
                yield return matrix[i, column];
            }
            yield break;
        }
        public IEnumerable<double> GetRow(int row)
        {
            row -= 1; // n - не индекс а номер ( n=1 -> n=0 )
            for (int i = 0; i < Width; i++)
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
        public void SwitchRow(int row1, int row2)
        {
            row1--; row2--;
            for (int i = 0; i < Width; i++)
            {
                double swap_var = matrix[row1, i];
                matrix[row1, i] = matrix[row2, i];
                matrix[row2, i] = swap_var;
            }
        }
        /// <summary>
        /// Переставляет 2 столбца матрицы
        /// </summary>
        /// <param name="column1">Номер первого столбца</param>
        /// <param name="column2">Номер второго столбца</param>
        public void SwitchColumn(int column1,int column2)
        {
            column1--; column2--;
            for (int i = 0; i < Height; i++)
            {
                double swap_var = matrix[i, column1];
                matrix[i, column1] = matrix[i, column2];
                matrix[i, column2] = swap_var;
            }
        }

        public void MultiplicateRow(int row,double k)
        {
            this.RowMap(row, a => a * k);
        }
        public void MultiplicateColumn(int column, double k)
        {
            this.ColumnMap(column, a => a * k);
        }

        /// <summary>
        /// Складывает 2 строки матрицы
        /// </summary>
        /// <param name="row1">Строка, к которой плюсуют другую</param>
        /// <param name="row2">Прибавляемая строка</param>
        public void AddRow(int row1, int row2,double k = 1)
        {
            row2--; row1--;
            for (int i = 0; i < Height; i++)
            {
                matrix[row1, i] += matrix[row2, i] * k;
            }
        }
        /// <summary>
        /// Складывает 2 столбца матрицы
        /// </summary>
        /// <param name="column1">Столбец к которому прибарляют</param>
        /// <param name="column2">Прибавляемый столбец</param>
        public void AddColumn(int column1, int column2,double k = 1)
        {
            column1--; column2--;
            for (int i = 0; i < Width; i++)
            {
                matrix[i, column1] += matrix[i, column2] * k;
            }
        }

        public void TransformToTringularForm()
        {
            for (int k = 1; k < Height; k++)
            {
                for (int i = k+1; i <= Height; i++)
                {
                    // хз что делать при делении на 0
                    if (this[k - 1, k - 1] == 0) throw new DivideByZeroException();

                    AddRow(i, k, -this[i - 1, k-1] / this[k-1, k-1]);
                }
            }
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
            if (m1.Width != m2.Height)
                throw new ArgumentException("Форма матриц не согласована");
            
            int res_w, res_h;
            res_w = m2.Width;
            res_h = m1.Height;

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
            // TODO: исправить
            TransformToTringularForm();
            double res = 1;
            for (int i = 0; i < Width; i++)
            {
                res*=this[i,i];
            }
            return res;
        }
    }
}
