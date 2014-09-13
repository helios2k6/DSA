using DSA.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
   [TestClass]
   public sealed class AVLTreeTests
   {
      [TestMethod]
      public void BasicAVLTreeTest()
      {
         var tree = new AVLTree<int>();
         Assert.AreEqual(0, tree.Height);

         for (int i = 0; i < 10; i++)
         {
            tree.Add(i);
         }

         Assert.AreEqual(4, tree.Height);
         Assert.AreEqual(10, tree.Size);

         for (int i = 0; i < 10; i++)
         {
            Assert.IsTrue(tree.Contains(i));
         }

         Assert.IsFalse(tree.Contains(11));

         for (int i = 0; i < 10; i++)
         {
            Assert.IsTrue(tree.Remove(i));
         }

         Assert.AreEqual(0, tree.Height);
         Assert.AreEqual(0, tree.Size);
      }
   }
}