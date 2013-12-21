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

    // TODO: Красно-черное дерево (и АВЛ дерево =) )
    public class BSTree<TKey,TData> : IBinaryTree<TData>
        where TKey: IComparable<TKey>
    {
        #region public propertys
        public TKey Key { get; set; }
        public TData Data { get; set; }
        public BSTree<TKey, TData> LeftNode {
            get { return left_node; }
            set
            {
                left_node = value;
                if (value != null) left_node.parent = this;
            }
        }
        public BSTree<TKey, TData> RightNode
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
        private BSTree<TKey, TData> parent;
        private BSTree<TKey, TData> left_node;
        private BSTree<TKey, TData> right_node;
        #endregion

        /// <summary>
        /// Выполняет поиск элемента дерева. Если элемент отсутствует бросает исключение.
        /// </summary>
        /// <param name="k">Ключь искомого элемента</param>
        /// <returns>Элемент с искомым ключем</returns>
        public BSTree<TKey, TData> Find(TKey k)
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
        /// <summary>
        /// Выполняет вставку элемента в дерево.
        /// При совпадении ключей обновляет значение
        /// </summary>
        /// <param name="k">Кючь вставляемого элемента</param>
        /// <param name="data">Значение элемента</param>
        public void Insert(TKey k, TData data)
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
                    LeftNode = new BSTree<TKey, TData>() // создание ветви 
                    { Key = k, Data = data, parent = this };
            }
            else
            {
                if (RightNode != null)
                    RightNode.Insert(k, data);
                else
                    RightNode = new BSTree<TKey, TData>() 
                    { Key = k, Data = data, parent = this };
            }
        }
        /// <summary>
        /// Удаляет элемент из дерева
        /// </summary>
        /// <param name="k">Удаляемый элемент</param>
        public void Remove(TKey k)
        {
            BSTree<TKey, TData> del_node;
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

        /// <summary>
        /// Обход дерева в порядке Левое поддерево -> Вершина -> Правое поддерево
        /// </summary>
        /// <param name="action">Действие, выполняемое с каждым элементом</param>
        public void Traverse_infix(Action<TKey, TData> action)
        {
            if (LeftNode != null) LeftNode.Traverse_infix(action);
            action(Key, Data);
            if (RightNode != null) RightNode.Traverse_infix(action);
        }
        /// <summary>
        /// Обход дерева в порядке Вершина -> Левое поддерево -> Правое поддерево
        /// </summary>
        /// <param name="action">Действие, выполняемое с каждым элементом</param>
        public void Traverse_prefix(Action<TKey, TData> action)
        {
            action(Key, Data);
            if (LeftNode != null) LeftNode.Traverse_prefix(action);
            if (RightNode != null) RightNode.Traverse_prefix(action);
        }
        /// <summary>
        /// Обход дерева в порядке Левое поддерево -> Правое поддерево -> Вершина
        /// </summary>
        /// <param name="action">Действие, выполняемое с каждым элементом</param>        
        public void Traverse_postfix(Action<TKey, TData> action)
        {
            if (LeftNode != null) LeftNode.Traverse_postfix(action);
            if (RightNode != null) RightNode.Traverse_postfix(action);
            action(Key, Data);
        }

        #region IBinaryTree<T> implementation
        IBinaryTree<TData> IBinaryTree<TData>.LeftNode
        {
            get { return LeftNode; }
            set { LeftNode = (BSTree<TKey,TData>)value; }
        }
        IBinaryTree<TData> IBinaryTree<TData>.RightNode
        {
            get { return RightNode; }
            set { RightNode = (BSTree<TKey,TData>)value; }
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
        public static bool IsCorrectBSTree(BSTree<TKey,TData> bstree)
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
