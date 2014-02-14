using System;
using System.Drawing;
using System.Numerics;

namespace HRUC.Math
{
    /// <summary>
    ///     Класс для представления комплексной плоскости
    ///     Соотносит комплексный координаты с координатами изображения
    ///     эквивалентного данному предаставлению
    /// </summary>
    [Serializable]
    public class ComplexPlane
        : ICloneable, IEquatable<ComplexPlane>
    {
        #region Fields

        private Complex _center;
        private double _delta;
        private int _height;
        private int _width;

        #endregion

        #region Constructors

        // исключительно для сериализации
        private ComplexPlane()
            : this(Complex.Zero, 0, 0, Complex.Zero)
        {
        }

        public ComplexPlane(double delta, int width, int height, Complex center)
        {
            _delta = delta;
            _width = width;
            _height = height;
            _center = center;
        }

        // Diff - диапазон (между max и min)
        public ComplexPlane(Complex diff, int width, int height, Complex center)
            : this(0, width, height, center)
        {
            //delta = Math.Max(Diff.Imaginary,Diff.Real) / Math.Max(width,height);
            Diff = diff;
        }

        #endregion

        /// <summary>
        ///     Получает или задает центральную точку
        /// </summary>
        public Complex Center
        {
            get { return _center; }
            set { _center = value; }
        }

        /// <summary>
        ///     Диапазон (разность между Max и Min)
        /// </summary>
        public Complex Diff
        {
            get { return new Complex(_delta*_width, _delta*_height); }
            set
            {
                // rewrite
                if (value.Real > value.Imaginary)
                {
                    _delta = value.Real/_width;
                }
                else
                {
                    _delta = value.Imaginary/_height;
                }
            }
        }

        /// <summary>
        ///     Величина одного пикселя
        /// </summary>
        public double Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        /// <summary>
        ///     Размер мнимой части комплексной плоскости
        /// </summary>
        public double ImmagSize
        {
            get { return _delta*_height; }
        }

        /// <summary>
        ///     Размер действительной части комплексной плоскости
        /// </summary>
        public double RealSize
        {
            get { return _delta*_width; }
        }

        /// <summary>
        ///     Ширина изображения (При изменении не меняет дельту)
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        ///     Высота изображения (При изменении не меняет дельту)
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        ///     Размер (в пикселях) плоскости
        /// </summary>
        public Size Size
        {
            get { return new Size(_width, _height); }
            set { Resize(value.Width, value.Height); }
        }

        /// <summary>
        ///     Индексатор, соотнесение пикселя и соответствующего комплексного числа
        /// </summary>
        /// <param name="w">номер пиксела по горизонтали</param>
        /// <param name="h">номер пиксела по вертикали</param>
        /// <returns></returns>
        public Complex this[int w, int h]
        {
            get { return GetComplex(w, h); }
        }

        public object Clone()
        {
            return new ComplexPlane(_delta, _width, _height, _center);
        }

        #region object override

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("delta = {0} width = {1} Height = {2} center = {3}"
                , _delta, _width, _height, _center);
        }

        #endregion

        #region Equals

        bool IEquatable<ComplexPlane>.Equals(ComplexPlane other)
        {
            return _delta == other._delta &&
                   _width == other._width &&
                   _center == other._center;
        }

        public static bool operator ==(ComplexPlane c1, ComplexPlane c2)
        {
            if ((object) c1 == null || (object) c2 == null)
            {
                return false;
            }
            return c1.Equals(c2);
        }

        public static bool operator !=(ComplexPlane c1, ComplexPlane c2)
        {
            if ((object) c1 == null ^ (object) c2 == null)
            {
                return true;
            }
            return !c1.Equals(c2);
        }

        public override bool Equals(object obj)
        {
            var other = (ComplexPlane) obj;
            return _delta == other._delta &&
                   _width == other._width &&
                   _center == other._center;
        }

        #endregion

        /// <summary>
        ///     Задает длину и ширину (в пикселях) изображения соответствующего
        ///     этой плоскости. Метод сохраняет текущую активную зону (в отличии от свойств width и Height)
        /// </summary>
        /// <param name="w">Новое значение ширины</param>
        /// <param name="h">Новое значение высоты</param>
        public void Resize(int w, int h)
        {
            // Меняет дельту?
            Complex c = Diff;
            _width = w;
            _height = h;
            Diff = c;
            // Меняет! но активная область всегда входит
        }

        public Complex GetComplex(int w, int h)
        {
            double re = MinReal + w*_delta;
            double im = MaxImag - h*_delta;
            return new Complex(re, im);
        }

        #region Max/Min

        // как неожиданно =)
        /// <summary>
        ///     Действительный максимум
        /// </summary>
        public double MaxReal
        {
            get { return _width*_delta/2 + _center.Real; }
        }

        /// <summary>
        ///     Действительный минимум
        /// </summary>
        public double MinReal
        {
            get { return _center.Real - _width*_delta/2; }
        }

        /// <summary>
        ///     Мнимый максимум
        /// </summary>
        public double MaxImag
        {
            get { return _height*_delta/2 + _center.Imaginary; }
        }

        /// <summary>
        ///     Мнимый минимум
        /// </summary>
        public double MinImag
        {
            get { return _center.Imaginary - _height*_delta/2; }
        }

        #endregion
    }
}