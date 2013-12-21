using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HRUC;

namespace HRUC_Tests
{
    [TestClass]
    public class BSTreeTests
    {
        [TestMethod]
        public void Insert_element()
        {
            BSTree<int, string> tree = new BSTree<int, string> { Key = 0, Data = "zero" };

            tree.Insert(-1, "-1");
            tree.Insert(2, "2");
            tree.Insert(1, "1");
            tree.Insert(3, "3");

            ///    0
            ///   / \
            /// -1   2
            ///     / \
            ///    1   3

            Assert.AreEqual("zero", tree.Data);
            Assert.AreEqual("3", tree.RightNode.RightNode.Data);
            Assert.AreEqual("1", tree.RightNode.LeftNode.Data);

        }

        [TestMethod]
        public void Find_element()
        {
            BSTree<int, string> tree = new BSTree<int, string>()
            {
                Data = "zero",
                Key = 0
            };

            tree.Insert(-1, "-1");
            tree.Insert(2, "2");
            tree.Insert(1, "1");
            tree.Insert(3, "3");

            ///    0
            ///   / \
            /// -1   2
            ///     / \
            ///    1   3
            ///    

            /// Act
            var find_result = tree.Find(2);

            Assert.AreEqual("2", find_result.Data);

            find_result = tree.Find(1);

            Assert.AreEqual("1", find_result.Data);
        }

        [TestMethod]
        public void Remove_element()
        {
            BSTree<int, string> tree = new BSTree<int, string>()
            {
                Data = "7",
                Key = 7
            };

            tree.Insert(3, "3");
            tree.Insert(8, "8");
            tree.Insert(2, "2");
            tree.Insert(5, "5");
            tree.Insert(9, "9");
            tree.Insert(4, "4");

            ///     7
            ///    / \
            ///   3   8
            ///  / \   \
            /// 2   5   9
            ///    /
            ///   4
            
            tree.Remove(3);

            Assert.AreEqual("4", tree.LeftNode.Data);
            Assert.AreEqual("5", tree.LeftNode.RightNode.Data);
        }
    }
}
