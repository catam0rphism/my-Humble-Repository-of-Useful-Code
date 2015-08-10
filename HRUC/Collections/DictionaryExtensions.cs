using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRUC.Collections
{
    public static class DictionaryExtensions
    {
        public static TDict Merge<TKey, TValue, TDict>(this TDict first, TDict second)
            where TDict : IDictionary<TKey, TValue>, new()
            where TValue : IComparable<TValue>
        {
            var tmp = new TDict();
            foreach (var item in first)
            {
                tmp.Add(item.Key, item.Value);
            }

            foreach (var item in second)
            {
                if (tmp.ContainsKey(item.Key) && !tmp[item.Key].Equals(item.Value))
                {
                    // do it?
                    throw new ArgumentException("Key already exist", "second");
                }

                tmp[item.Key] = item.Value;
            }
            return tmp;
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second)
            where TValue : IComparable<TValue>
        {
            return Merge<TKey, TValue, Dictionary<TKey, TValue>>(first, second);
        }
    }
}
