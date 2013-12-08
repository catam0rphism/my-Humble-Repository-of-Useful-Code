using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLUT
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source,Action<T> func)
        {
            foreach (T item in source)
            {
                func(item);
            }
        }
    }
}
