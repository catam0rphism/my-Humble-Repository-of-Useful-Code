using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HRUC.Images
{
    /// <summary>
    /// Класс, реализующий быстрый доступ к ручному редактированию пикселей
    /// т.к. методы GetPixel() и SetPixel() жутко медленные
    /// </summary>
    public class SpeedBitmap
    {
        public SpeedBitmap(Image img)
        {
            _img = new Bitmap(img);
        }
        public SpeedBitmap(Bitmap img)
        {
            _img = img;
            //_img = (Bitmap)img.Clone();
        }
        public SpeedBitmap(Size size)
        {
            _img = new Bitmap(size.Width,size.Height);
        }
        public SpeedBitmap(int width, int height)
        {
            _img = new Bitmap(width, height);
        }

        Bitmap _img;
        BitmapData _imgData;
        byte[] _imagePixelArray;

        /// <summary>
        /// Размер (в байтах) одного пиксела img
        /// </summary>
        const int ColorSize = 4;

        public Bitmap Image
        {
            get { return _img; }
            set { _img = value; }
        }

        public void LockBitmap()
        {
            // Лочит все изображение, при больших размерах есть вероятность OutOfMemory
            // TODO: Поправить (или создать дополнительные методы для частичной блокировки)
            _imgData = _img.LockBits(new Rectangle(Point.Empty, _img.Size), 
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            copyPixelsToArray();
        }
        public void LockBitmap(Rectangle rectangle)
        {
            _imgData = _img.LockBits(
                rectangle, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            copyPixelsToArray();
        }

        public void UnlockBitmap()
        {
            // Копируем измененные данные обратно в изображение
            Marshal.Copy(
                source: _imagePixelArray,
                destination: _imgData.Scan0,
                startIndex: 0,
                length: _imagePixelArray.Length);

            _img.UnlockBits(_imgData);
        }

        private void copyPixelsToArray()
        {
            // Инициализируем массив для пикселей img
            _imagePixelArray = new byte[_imgData.Height * _imgData.Stride];

            // Заполняем массив
            Marshal.Copy(
                source: _imgData.Scan0,
                destination: _imagePixelArray,
                startIndex: 0,
                length: _imagePixelArray.Length);
        }

        public Color GetPixel(int x, int y)
        {
            int start_index = _imgData.Stride * y + ColorSize * x;
            
            byte b = _imagePixelArray[start_index];
            byte g = _imagePixelArray[start_index + 1];
            byte r = _imagePixelArray[start_index + 2];
            byte a = _imagePixelArray[start_index + 3];

            return Color.FromArgb(a, r, g, b);            
        }

        public void SetPixel(int x, int y, Color c)
        {
            int start_index = _imgData.Stride * y + ColorSize * x;

            _imagePixelArray[start_index] = c.B;
            _imagePixelArray[start_index + 1] = c.G;
            _imagePixelArray[start_index + 2] = c.R;
            _imagePixelArray[start_index + 3] = c.A;
        }

        public Color this[int x, int y]
        {
            get { return GetPixel(x, y); }
            set { SetPixel(x, y, value); }
        }
    }
}
