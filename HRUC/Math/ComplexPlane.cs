using System;
using System.Drawing;
using System.Numerics;
using System.Collections.Generic;

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

        private readonly Complex _center;
        private readonly double _delta;
        private readonly int _height;
        private readonly int _width;

        #endregion

        #region Constructors

        // исключительно для сериализации
        public ComplexPlane()
            : this(Complex.Zero, 0, 0, Complex.Zero) { }

        public ComplexPlane(double delta, int width, int height, Complex center)
        {
            if (delta == double.NaN
                || center == new Complex(double.NaN, double.NaN)
                || double.IsInfinity(delta)
                || double.IsInfinity(center.Real)
                || double.IsInfinity(center.Imaginary))
                    throw new ArgumentException("delta or center has invalid value");
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
            if (diff.Real > diff.Imaginary)
            {
                _delta = diff.Real / _width;
            }
            else
            {
                _delta = diff.Imaginary / _height;
            }
        }

        #endregion

        /// <summary>
        ///     Получает центральную точку
        /// </summary>
        public Complex Center
        {
            get { return _center; }
        }
        /// <summary>
        ///     Создает новый объект ComplexPlane с центром, переданным в параметре
        /// </summary>
        /// <param name="Center">Значение центра части комплексной плоскости</param>
        public ComplexPlane SetCenter(Complex Center)
        {
            return new ComplexPlane(_delta, _width, _height, Center);
        }

        /// <summary>
        ///     Диапазон (разность между Max и Min)
        /// </summary>
        public Complex Diff
        {
            get { return new Complex(_delta*_width, _delta*_height); }
        }
        /// <summary>
        ///     Создает новый объект ComplexPlane используя диапазон, переданный в параметре
        /// </summary>
        /// <param name="Difference">Значение диапазона</param>
        public ComplexPlane SetDifference(Complex Difference)
        {
            return new ComplexPlane(Difference, _width, _height, _center);
        }

        /// <summary>
        ///     Величина одного пикселя
        /// </summary>
        public double Delta
        {
            get { return _delta; }
        }
        /// <summary>
        ///     Создает новый объект ComplexPlane используя шаг дескритизации, переданный в параметре
        /// </summary>
        /// <param name="Delta">Шаг дескритизации</param>
        public ComplexPlane SetDelta(double Delta)
        {
            return new ComplexPlane(Delta, _width, _height, _center);
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
        ///     Ширина изображения
        /// </summary>
        public int Width
        {
            get { return _width; }
        }
        // не меняет дельту
        public ComplexPlane SetWidth(int Width)
        {
            return new ComplexPlane(_delta, Width, _height, _center);
        }

        /// <summary>
        ///     Высота изображения
        /// </summary>
        public int Height
        {
            get { return _height; }
        }
        public ComplexPlane SetHeight(int Height)
        {
            return new ComplexPlane(_delta, _width, Height, _center);
        }

        /// <summary>
        ///     Размер (в пикселях) плоскости
        /// </summary>
        public Size Size
        {
            get { return new Size(_width, _height); }
        }

        /// <summary>
        ///     Получает комплексное число соответствующее пикселю
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
        public ComplexPlane Resize(int w, int h)
        {
            return new ComplexPlane(Diff, w, h, _center);
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