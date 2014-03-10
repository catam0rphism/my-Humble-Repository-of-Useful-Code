#define DEBUG

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRUC.Images
{
    /// <summary>
    /// Реализует методы построения гистограм изображения
    /// </summary>
    public static class Histogram
    {
        /// <summary>
        /// Тип гистограмы
        /// </summary>
        public enum HistogramType
        {
            /// <summary>
            /// Гистограма яркости
            /// </summary>
            Brightness,
            /// <summary>
            /// Гисторама красного канала
            /// </summary>
            Red,
            /// <summary>
            /// Гистограма зеленого канала
            /// </summary>
            Green,
            /// <summary>
            /// Гистограма синего канало
            /// </summary>
            Blue
        }
        /// <summary>
        /// Генерирует гистограму изображения
        /// </summary>
        /// <param name="image">Изображение</param>
        /// <param name="histoType">Тип гистограмы</param>
        /// <returns>Массив в 256 элементов, где каждый элемент соответствует числу пикселей</returns>
        public static int[] MakeHistogram(Image image, HistogramType histoType)
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
                    
                    int ind = -1;
                    switch (histoType)
                    {
                        case HistogramType.Brightness:
                            ind = (int)(c.GetBrightness() * (levelCount-1));
                            //ind = (int)System.Math.Sqrt(c.R * c.R * 0.241 + c.G * c.G * 0.691 + c.B * c.B * 0.068);
                            break;
                        case HistogramType.Red:
                            ind = c.R;
                            break;
                        case HistogramType.Green:
                            ind = c.G;
                            break;
                        case HistogramType.Blue:
                            ind = c.B;
                            break;
                        default:
                            break;
                    }
                    
                    histo[ind]++;
                }
            }

            img.UnlockBitmap();
                        
            return histo;
        }
        /// <summary>
        /// Генерирует изображение гистограмы
        /// </summary>
        /// <param name="image">Изображение, гистограму которого требуется построить</param>
        /// <param name="histoType">Тип гистограмы</param>
        /// <param name="histoSize">Размер гистограмы</param>
        /// <returns>Гистограма, размера histoSize требуемого канала</returns>
        public static Image MakeHistogram(Image image,HistogramType histoType, Size histoSize)
        {
            int levelCount = 256;
            Bitmap img = new Bitmap(histoSize.Width, histoSize.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                int[] histo = MakeHistogram(image,histoType);

                float dx = (float)histoSize.Width / (float)levelCount;
                float dy = (float)histoSize.Height / (float)histo.Max();
                g.Clear(Color.White);

                Color color = histoType == HistogramType.Brightness ? Color.Black
                    : histoType == HistogramType.Red ? Color.Red
                    : histoType == HistogramType.Green ? Color.Green
                    : histoType == HistogramType.Blue ? Color.Blue
                    : Color.HotPink; // Никогда не сработает

                for (int i = 0; i < levelCount; i++)
                {
                    // Рисуем развернутую гистограму
                    g.FillRectangle(new SolidBrush(color),
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
