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
        
        /// 1 2 3
        /// 4 5 6
        /// element [1,0] == 4
        /// element [0,2] == 3
        /// TODO: Make test )
        public double this[int h,int w]
        {
            get {return matrix[h,w];}
            set {matrix[h,w] = value;}
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

        public IEnumerable<double> GetColumn(int n)
        {
            n -= 1; // n - не индекс а номер ( n=1 -> n=0 )
            for (int i = 0; i < Height; i++)
            {
                yield return matrix[i, n];
            }
            yield break;
        }
        public IEnumerable<double> GetRow(int n)
        {
            n -= 1; // n - не индекс а номер ( n=1 -> n=0 )
            for (int i = 0; i < Width; i++)
            {
                yield return matrix[n, i];
            }
            yield break;
        }
        
        private double[,] matrix;

        // TODO: Функции высшего порядка для матриц, реализация операций через них
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
    }
}
