using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRUC
{
    public interface IBinaryTree<T>
    {
        T Data { get; set; }
        IBinaryTree<T> LeftNode { get; set; }
        IBinaryTree<T> RightNode { get; set; }
        bool IsEmpty { get; }
    }

    public class BSTree<TKey,TValue> : IBinaryTree<TValue>
        where TKey: IComparable<TKey>
    {
        public TKey Key { get; set; }
        public TValue Data { get; set; }
        public BSTree<TKey, TValue> LeftNode { get; set; }
        public BSTree<TKey, TValue> RightNode { get; set; }

        #region IBinaryTree<T>
        IBinaryTree<TValue> IBinaryTree<TValue>.LeftNode
        {
            get { return LeftNode; }
            set { LeftNode = (BSTree<TKey,TValue>)value; }
        }
        IBinaryTree<TValue> IBinaryTree<TValue>.RightNode
        {
            get { return RightNode; }
            set { RightNode = (BSTree<TKey,TValue>)value; }
        }
        bool IBinaryTree<TValue>.IsEmpty
        {
            get { return LeftNode == null || RightNode == null; }
        }
        #endregion

    }

    // Реализация Collections.Generic.KeyValuePair<TKey,TValue> - структура. От нее нельзя наследовать
    public interface IKeyValuePair<TKey, TValue>
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
    }
}
