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

        public BSTree<TKey, TValue> Find(TKey k)
        {
            var compare_result = Key.CompareTo(k);
            if(compare_result == 0)
            {
                return this;
            }
            else if(compare_result < 0)
            {
                if (RightNode != null)
                    return RightNode.Find(k);
                else throw new InvalidOperationException("Элемента нету! уперли!");
            }
            else 
            {
                if (LeftNode != null)
                    return LeftNode.Find(k);
                else throw new InvalidOperationException("Элемента нету! уперли!");
            }
        }
        public void Insert(TKey k, TValue data)
        {
            var compare_result = Key.CompareTo(k);

            if (compare_result == 0)
            {
                this.Data = data; // обновление данных при совпадении ключа
            }
            else if (compare_result > 0)
            {
                if (LeftNode != null)
                    LeftNode.Insert(k, data); // рекурсивный поиск
                else
                    LeftNode = new BSTree<TKey, TValue>() // создание ветви 
                    { Key = k, Data = data };
            }
            else
            {
                if (RightNode != null)
                    RightNode.Insert(k, data);
                else
                    RightNode = new BSTree<TKey, TValue>() { Key = k, Data = data };
            }
        }

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

        /// <summary>
        /// Метод проверки корректности двоичного дерева поиска
        /// </summary>
        /// <param name="bstree">Проверяемое дерево</param>
        /// <returns></returns>
        public static bool IsCorrectBSTree(BSTree<TKey,TValue> bstree)       
        {            
            Func<bool> compareWithLeftTree = () => bstree.LeftNode.Key.CompareTo(bstree.Key) < 0;
            Func<bool> compareWithRightTree = () => bstree.Key.CompareTo(bstree.RightNode.Key) <= 0;

            // Рекурсия =)
            if (bstree.LeftNode == null && bstree.RightNode == null)
                return true; // выход из рекурсии

            if (bstree.LeftNode == null && bstree.RightNode != null)
                return compareWithRightTree() && IsCorrectBSTree(bstree.RightNode);

            if (bstree.RightNode == null && bstree.LeftNode != null)
                return compareWithLeftTree() && IsCorrectBSTree(bstree.LeftNode);

            return compareWithLeftTree() // Верно левая ветвь
                && compareWithRightTree() // Правая ветвь
                && IsCorrectBSTree(bstree.LeftNode) // Левое дерево
                && IsCorrectBSTree(bstree.RightNode); // Правое дерево

        }
    }
}
