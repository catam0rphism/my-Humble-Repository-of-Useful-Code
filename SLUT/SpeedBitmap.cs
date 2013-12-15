using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT
{
    /// <summary>
    /// Класс, реализующий быстрый доступ к ручному редактированию пикселей
    /// т.к. методы GetPixel() и SetPixel() жутко медленные
    /// </summary>
    class SpeedBitmap
    {
        Bitmap img;
        BitmapData imgData;
        Bitmap Image
        {
            get { return img; }
            set { img = value; }
        }

        public void LockBitmap()
        {
            // Лочит все изображение, при больших размерах есть вероятность OutOfMemory
            imgData = img.LockBits(new Rectangle(Point.Empty, img.Size), 
                ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        }
        public void UnlockBitmap()
        {
            img.UnlockBits(imgData);
        }
    }
}
