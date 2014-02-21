using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRUC.Images
{
    public class Histogram
    {

        public static int[] MakeHistogram(Image image)
        {
            int levelCount = 256;
            // Массив, содержащий число пикселей по уровням яркости
            int[] histo = new int[levelCount];

            SpeedBitmap img = new SpeedBitmap(image);

            img.LockBitmap();

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = img[i, j];
                    // Яркость цвета - индекс в выходном массиве
                    int ind = (int)(c.GetBrightness() * (levelCount-1));

                    histo[ind]++;
                }
            }

            img.UnlockBitmap();

            return histo;
        }
        public static Image MakeHistogram(Image image, Size histoSize)
        {
            int levelCount = 256;
            Bitmap img = new Bitmap(histoSize.Width, histoSize.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                int[] histo = MakeHistogram(image);

                float dx = (float)histoSize.Width / (float)levelCount;
                float dy = (float)histoSize.Height / (float)histo.Max();
                g.Clear(Color.White);

                for (int i = 0; i < levelCount; i++)
                {
                    // Рисуем развернутую гистограму
                    g.FillRectangle(new SolidBrush(Color.Black),
                        x: i * dx,
                        y:  0,
                        width: dx,
                        height: (float)histo[i] * dy);
                }
                                
            }

            img.RotateFlip(RotateFlipType.Rotate180FlipX);
            return img;
        }
    }
}
