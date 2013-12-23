//using System.Xml.Serialization;
using System;
using System.Numerics;

namespace HRUC.Math
{
    /// <summary>
    /// Класс для представления комплексной плоскости
    /// Соотносит комплексный координаты с координатами изображения
    /// эквивалентного данному предаставлению
    /// </summary>
    [Serializable]
    public class ComplexPlane
        : ICloneable, IEquatable<ComplexPlane>
    {
        #region Fields
        double delta;
        int width;
        int height;
        Complex center;
        #endregion
        #region Constructors
        // исключительно для сериализации
        public ComplexPlane()
            :this(Complex.Zero, 0, 0, Complex.Zero) { }

        public ComplexPlane(double delta, int Width, int Height, Complex Center)
        {
            this.delta = delta;
            this.width = Width;
            this.height = Height;
            this.center = Center;
        }
        // Diff - диапазон (между max и min)
        public ComplexPlane(Complex Diff, int Width, int Height, Complex Center)
            :this(0, Width, Height, Center)
        {
            //delta = Math.Max(Diff.Imaginary,Diff.Real) / Math.Max(width,height);
            this.Diff = Diff;
        }
        #endregion

        /// <summary>
        /// Получает или задает центральную точку
        /// </summary>
        public Complex Center { get { return center; } set { center = value; } }

        /// <summary>
        /// Диапазон (разность между Max и Min)
        /// </summary>
        public Complex Diff
        {
            get { return new Complex(delta * width, delta * height); }
            set 
            {
                // rewrite
                if (value.Real > value.Imaginary)
                {
                    delta = value.Real / width;
                }
                else
                {
                    delta = value.Imaginary / height;
                }
            }
        }

        /// <summary>
        /// Величина одного пикселя
        /// </summary>
        public double Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        /// <summary>
        /// Размер мнимой части комплексной плоскости
        /// </summary>
        public double ImmagSize
        {
            get { return delta * height; }
        }
        /// <summary>
        /// Размер действительной части комплексной плоскости
        /// </summary>
        public double RealSize
        {
            get { return delta * width; }
        }
        /// <summary>
        /// Ширина изображения (При изменении не меняет дельту)
        /// </summary>
        public int Width
        {
            get { return width; }
            set 
            {
                width = value;
            }
        }
        /// <summary>
        /// Высота изображения (При изменении не меняет дельту)
        /// </summary>
        public int Height
        {
            get { return height; }
            set 
            { 
                height = value;
            }
        }
        /// <summary>
        /// Размер (в пикселях) плоскости
        /// </summary>
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(width, height); }
            set { Resize(value.Width, value.Height); }
        }
        /// <summary>
        /// Задает длину и ширину (в пикселях) изображения соответствующего
        /// этой плоскости. Метод сохраняет текущую активную зону (в отличии от свойств Width и Height)
        /// </summary>
        /// <param name="w">Новое значение ширины</param>
        /// <param name="h">Новое значение высоты</param>
        public void Resize(int w, int h)
        {
            // Меняет дельту?
            Complex c = Diff;
            width = w;
            height = h;
            Diff = c;
            // Меняет! но активная область всегда входит
        }

        /// <summary>
        /// Индексатор, соотнесение пикселя и соответствующего комплексного числа
        /// </summary>
        /// <param name="w">номер пиксела по горизонтали</param>
        /// <param name="h">номер пиксела по вертикали</param>
        /// <returns></returns>
        public Complex this[int w, int h] { get { return GetComplex(w, h); } }        
        public Complex GetComplex(int w, int h)
        {
            double re = MinReal + w * delta;
            double im = MaxImag - h * delta;
            return new Complex(re, im);
        }

        #region Max/Min
        // как неожиданно =)
        /// <summary>
        /// Действительный максимум
        /// </summary>
        public double MaxReal
        {
            get { return width * delta / 2 + center.Real; }
        }
        /// <summary>
        /// Действительный минимум
        /// </summary>
        public double MinReal
        {
            get { return center.Real - width * delta /2; }
        }
        /// <summary>
        /// Мнимый максимум
        /// </summary>
        public double MaxImag
        {
            get { return height * delta / 2 + center.Imaginary; }
        }
        /// <summary>
        /// Мнимый минимум
        /// </summary>
        public double MinImag
        {
            get { return center.Imaginary - height * delta / 2; }
        }
        #endregion

        public object Clone()
        {
            return new ComplexPlane(delta, width, height, center);
        }        
        #region object override
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("delta = {0} Width = {1} Height = {2} center = {3}"
                , delta, width, height, center);
        }
        #endregion
        #region Equals
        public static bool operator ==(ComplexPlane c1, ComplexPlane c2)
        {
            if ((object)c1 == null || (object)c2 == null)
            {
                return false;
            }
            return c1.Equals(c2);
        }
        public static bool operator !=(ComplexPlane c1, ComplexPlane c2)
        {
            if ((object)c1 == null ^ (object)c2 == null)
            {
                return true;
            }
            return !c1.Equals(c2);
        }
        public override bool Equals(object obj)
        {
            ComplexPlane other = (ComplexPlane)obj;
            return this.delta == other.delta &&
                   this.width == other.width &&
                   this.center == other.center;
        }
        bool IEquatable<ComplexPlane>.Equals(ComplexPlane other)
        {
            return this.delta == other.delta &&
                   this.width == other.width &&
                   this.center == other.center;
        }
        #endregion
    }
}
