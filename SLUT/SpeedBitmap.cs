using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace HRUC
{
    /// <summary>
    /// Класс, реализующий быстрый доступ к ручному редактированию пикселей
    /// т.к. методы GetPixel() и SetPixel() жутко медленные
    /// </summary>
    class SpeedBitmap
    {
        Bitmap img;
        BitmapData img_data;
        byte[] image_pixel_array;

        /// <summary>
        /// Размер (в байтах) одного пиксела img
        /// </summary>
        const int color_size = 4;

        Bitmap Image
        {
            get { return img; }
            set { img = value; }
        }

        public void LockBitmap()
        {
            // Лочит все изображение, при больших размерах есть вероятность OutOfMemory
            // TODO: Поправить (или создать дополнительные методы для частичной блокировки)
            img_data = img.LockBits(new Rectangle(Point.Empty, img.Size), 
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            copyPixelsToArray();
        }
        public void LockBitmap(Rectangle rectangle)
        {
            img_data = img.LockBits(
                rectangle, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            copyPixelsToArray();
        }

        public void UnlockBitmap()
        {
            // Копируем измененные данные обратно в изображение
            Marshal.Copy(
                source: image_pixel_array,
                destination: img_data.Scan0,
                startIndex: 0,
                length: image_pixel_array.Length);

            img.UnlockBits(img_data);
        }

        private void copyPixelsToArray()
        {
            // Инициализируем массив для пикселей img
            image_pixel_array = new byte[img_data.Height * img_data.Stride];

            // Заполняем массив
            Marshal.Copy(
                source: img_data.Scan0,
                destination: image_pixel_array,
                startIndex: 0,
                length: image_pixel_array.Length);
        }

        public Color GetPixel(int x, int y)
        {
            int start_index = img_data.Height * y + color_size * x;
            
            byte b = image_pixel_array[start_index];
            byte g = image_pixel_array[start_index + 1];
            byte r = image_pixel_array[start_index + 2];
            byte a = image_pixel_array[start_index + 3];

            return Color.FromArgb(a, r, g, b);            
        }

        public void SetPixel(int x, int y, Color c)
        {
            int start_index = img_data.Height * y + color_size * x;

            image_pixel_array[start_index] = c.B;
            image_pixel_array[start_index + 1] = c.G;
            image_pixel_array[start_index + 2] = c.R;
            image_pixel_array[start_index + 3] = c.A;
        }

        public Color this[int x, int y]
        {
            get { return GetPixel(x, y); }
            set { SetPixel(x, y, value); }
        }
    }
}
