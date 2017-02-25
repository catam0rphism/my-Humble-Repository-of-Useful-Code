using System;
using System.Collections.Generic;

namespace HRUC.Collections
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
            get { return _leftNode; }
            set
            {
                _leftNode = value;
                if (value != null) _leftNode._parent = this;
            }
        }
        public BSTree<TKey, TData> RightNode
        {
            get
            { return _rightNode; }
            set
            {
                _rightNode = value;
                if (value != null) _rightNode._parent = this;
            }
        }
        #endregion
        #region fields
        // открытого parent не будет!
        private BSTree<TKey, TData> _parent;
        private BSTree<TKey, TData> _leftNode;
        private BSTree<TKey, TData> _rightNode;
        #endregion

        /// <summary>
        /// Выполняет поиск элемента дерева. Если элемент отсутствует бросает исключение.
        /// </summary>
        /// <param name="k">Ключь искомого элемента</param>
        /// <returns>Элемент с искомым ключем</returns>
        public BSTree<TKey, TData> Find(TKey k)
        {
            var compareResult = Key.CompareTo(k);
            if(compareResult == 0)
            {
                return this;
            }
            if(compareResult < 0)
            {
                if (RightNode != null)
                    return RightNode.Find(k);
                throw new InvalidOperationException("Искомый элемент отсутствует");
            }
            if (LeftNode != null)
                return LeftNode.Find(k);
            throw new InvalidOperationException("Искомый элемент отсутствует");
        }
        /// <summary>
        /// Выполняет вставку элемента в дерево.
        /// При совпадении ключей обновляет значение
        /// </summary>
        /// <param name="k">Кючь вставляемого элемента</param>
        /// <param name="data">Значение элемента</param>
        public void Insert(TKey k, TData data)
        {
            var compareResult = Key.CompareTo(k);

            if (compareResult == 0)
            {
                Data = data; // обновление данных при совпадении ключа
            }
            else if (compareResult > 0)
            {
                if (LeftNode != null)
                    LeftNode.Insert(k, data); // рекурсивный поиск
                else
                    LeftNode = new BSTree<TKey, TData> // создание ветви 
                    { Key = k, Data = data, _parent = this };
            }
            else
            {
                if (RightNode != null)
                    RightNode.Insert(k, data);
                else
                    RightNode = new BSTree<TKey, TData> { Key = k, Data = data, _parent = this };
            }
        }
        // Для поддержки инициализаторов коллекций
        public void Add(TKey k, TData data)
        {
            Insert(k, data);
        }
        /// <summary>
        /// Удаляет элемент из дерева
        /// </summary>
        /// <param name="k">Удаляемый элемент</param>
        public void Remove(TKey k)
        {
            BSTree<TKey, TData> delNode;
            try
            {
                delNode = Find(k);
            }
            catch(InvalidOperationException)
            {
                throw new InvalidOperationException("Удаляемый элемент отсутствует");
            }

            var delParent = delNode._parent;

            if (delNode.IsLeaf)
            {
                // Удаляем del_node

                if (delParent.Key.CompareTo(delNode.Key) > 0)
                    delParent.LeftNode = null;
                else delParent.RightNode = null;

                // Необходимо?
                // del_node.parent = null;
                // сборщик мусора и так очистит
            }
            // есть только одна дочерняя нода
            else if (delNode.LeftNode == null ^ delNode.RightNode == null)
            {
                // Вставляем дочернюю на место удаляемой
                if (delParent.Key.CompareTo(delNode.Key) > 0)
                {
                    delParent.LeftNode = delNode.LeftNode ?? delNode.RightNode;
                }
                else
                {
                    delParent.LeftNode = delNode.LeftNode ?? delNode.RightNode;
                }
            }
            // обе дочерние ветви существуют
            else if(delNode.LeftNode != null && delNode.RightNode!=null)
            {
                // если есть правый потомок левого дерева
                if (delNode.RightNode.LeftNode != null)
                {
                    // обменять значение и ключь
                    delNode.Data = delNode.RightNode.LeftNode.Data;
                    delNode.Key = delNode.RightNode.LeftNode.Key;

                    // рекурсивно удалить потомка
                    delNode.RightNode.LeftNode.Remove(delNode.RightNode.LeftNode.Key);
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
    
    // TODO: test it
    public static class TreeUtils
    {
        public static List<T> TreeSort<T>(IEnumerable<T> collection)
            where T: IComparable<T>
        {
            BSTree<T, object> tmpTree = new BSTree<T, object>();

            foreach (var number in collection)
            {
                tmpTree.Insert(number, default(object)); // omfg default(object)
                // performance problem?
            }

            List<T> result = new List<T>();
            tmpTree.Traverse_infix((num, _) => { result.Add(num); });

            return result;            
        }
    }
}
