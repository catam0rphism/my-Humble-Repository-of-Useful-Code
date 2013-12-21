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
        bool IsLeaf { get; }
    }

    public class BSTree<TKey,TValue> : IBinaryTree<TValue>
        where TKey: IComparable<TKey>
    {
        #region public propertys
        public TKey Key { get; set; }
        public TValue Data { get; set; }
        public BSTree<TKey, TValue> LeftNode {
            get { return left_node; }
            set
            {
                left_node = value;
                if (value != null) left_node.parent = this;
            }
        }
        public BSTree<TKey, TValue> RightNode
        {
            get
            { return right_node; }
            set
            {
                right_node = value;
                if (value != null) right_node.parent = this;
            }
        }
        #endregion
        #region fields
        // открытого parent не будет!
        private BSTree<TKey, TValue> parent;
        private BSTree<TKey, TValue> left_node;
        private BSTree<TKey, TValue> right_node;
        #endregion

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
                else throw new InvalidOperationException("Искомый элемент отсутствует");
            }
            else 
            {
                if (LeftNode != null)
                    return LeftNode.Find(k);
                else throw new InvalidOperationException("Искомый элемент отсутствует");
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
                    { Key = k, Data = data, parent = this };
            }
            else
            {
                if (RightNode != null)
                    RightNode.Insert(k, data);
                else
                    RightNode = new BSTree<TKey, TValue>() 
                    { Key = k, Data = data, parent = this };
            }
        }
        public void Remove(TKey k)
        {
            BSTree<TKey, TValue> del_node;
            try
            {
                del_node = Find(k);
            }
            catch(InvalidOperationException)
            {
                throw new InvalidOperationException("Удаляемый элемент отсутствует");
            }

            var del_parent = del_node.parent;

            if (del_node.IsLeaf)
            {
                // Удаляем del_node

                if (del_parent.Key.CompareTo(del_node.Key) > 0)
                    del_parent.LeftNode = null;
                else del_parent.RightNode = null;

                // Необходимо?
                // del_node.parent = null;
                // сборщик мусора и так очистит
            }
            // есть только одна дочерняя нода
            else if (del_node.LeftNode == null ^ del_node.RightNode == null)
            {
                // Вставляем дочернюю на место удаляемой
                if (del_parent.Key.CompareTo(del_node.Key) > 0)
                {
                    del_parent.LeftNode = del_node.LeftNode ?? del_node.RightNode;
                }
                else
                {
                    del_parent.LeftNode = del_node.LeftNode ?? del_node.RightNode;
                }
            }
            // обе дочерние ветви существуют
            else
            {
                // если есть правый потомок левого дерева
                if (del_node.RightNode.LeftNode != null)
                {
                    // обменять значение и ключь
                    del_node.Data = del_node.RightNode.LeftNode.Data;
                    del_node.Key = del_node.RightNode.LeftNode.Key;

                    // рекурсивно удалить потомка
                    del_node.RightNode.LeftNode.Remove(del_node.RightNode.LeftNode.Key);
                }
            }
        }

        #region IBinaryTree<T> implementation
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
        public bool IsLeaf
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
                return compareWithRightTree() // правый ключь больше конрня
                    && IsCorrectBSTree(bstree.RightNode); // рекурсивно для правой ветви

            if (bstree.RightNode == null && bstree.LeftNode != null)
                return compareWithLeftTree() // левый ключь меньше корня
                    && IsCorrectBSTree(bstree.LeftNode); // рекурсивно для левой ветви

            return compareWithLeftTree() // левый ключь меньше корня
                && compareWithRightTree() // правый ключь больше корня
                && IsCorrectBSTree(bstree.LeftNode) // рекурсивный вызов для левой ветви
                && IsCorrectBSTree(bstree.RightNode); // .. для правой ветви

        }
    }
}
