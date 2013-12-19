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
    }

    public struct BSTree<T> : IBinaryTree<T>
        where T: IComparable<T>
    {
        public T Data
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public BSTree<T> LeftNode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public BSTree<T> RightNode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #region IBinaryTree<T>
        IBinaryTree<T> IBinaryTree<T>.LeftNode
        {
            get { return LeftNode; }
            set { LeftNode = (BSTree<T>)value; }
        }

        IBinaryTree<T> IBinaryTree<T>.RightNode
        {
            get { return RightNode; }
            set { RightNode = (BSTree<T>)value; }
        }
        #endregion
    }
}
