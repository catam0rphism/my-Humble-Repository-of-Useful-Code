using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMath = System.Math;

namespace HRUC.Math
{
    /// <summary>
    /// Класс для создания списка промежуточных значений различными методами интерполяции
    /// </summary>
    //Технически это апроксимация, ну да ладно :)
    public static class Interpolation
    {
        static List<int> LinearInterpolation(int min, int max, int length)
        {
            var res = new List<int>(length);
            double d = (double)(max - min) / (double)length;
            for (int i = 0; i < length; i++)
            {
                res.Add((int)System.Math.Round(i * d) + min);
            }
            return res;
        }
        static List<double> LinearInterpolation(double min, double max, int length)
        {
            var res = new List<double>(length);
            double d = (max - min) / (double)length;
            for (int i = 0; i < length; i++)
            {
                res.Add(i * d + min);
            }
            return res;
        }
        static List<double> CosinusInterpolation(double min, double max, int length)
        {
            var res = new List<double>(length);

            // Коэфициент приближения к min и max, k - [0;1]
            double k;
            // Шаг приращения k
            double dk = 1 / (double)length;
            for (k = 0;k <= 1 ; k+=dk)
            {
                res.Add((max+min+(max-min)*SMath.Cos(SMath.PI * k))/2d);
            }
            return res;
        }
        static List<int> CosinusInterpolation(int min, int max, int length)
        {
            var res = new List<int>(length);

            // Коэфициент приближения к min и max, k => [0;1]
            double k;
            // Шаг приращения k
            double dk = 1 / (double)length;
            for (k = 0; k <= 1; k += dk)
            {
                res.Add((int)((max + min + (max - min) * SMath.Cos(SMath.PI * k)) / 2d));
            }
            return res;
        }
    }
}
