using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT
{
    public class Matrix
    {
        /// <summary>
        /// Создает матрицу определенного размера
        /// </summary>
        /// <param name="width">ширина матрицы</param>
        /// <param name="height">высота матрицы</param>
        public Matrix(int width,int height)
        {
            matrix = new double[width, height];            
        }

        /// <summary>
        /// Создает матрицу на основе двумерного массива
        /// </summary>
        /// <param name="matrix_data">Массив элементов матрицы</param>
        public Matrix(double[,] matrix_data)
        {
            matrix = matrix_data;
        }

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
    }
}
