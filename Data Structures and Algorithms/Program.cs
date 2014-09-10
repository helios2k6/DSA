using System;
using DSA.Tree;

namespace DSA
{
   public static class Program
   {
      private static void Assert(bool predicate)
      {
         if (predicate == false)
         {
            throw new InvalidOperationException();
         }
      }

      private static void PrintTree<T>(IBinarySearchTree<T> tree) where T : IComparable<T>, IEquatable<T>
      {
         tree.InOrderTraversal(item => Console.WriteLine(item));
      }

      private static void BasicAVLTreeTest()
      {
         var tree = new AVLTree<int>();
         Assert(tree.Height == 0);

         for (int i = 0; i < 10; i++)
         {
            tree.Add(i);
         }

         Assert(tree.Height <= 4);
         Assert(tree.Size == 10);

         PrintTree(tree);

         for (int i = 0; i < 10; i++)
         {
            Assert(tree.Contains(i));
         }

         Assert(tree.Contains(11) == false);

         for (int i = 0; i < 10; i++)
         {
            Assert(tree.Remove(i));
         }

         Assert(tree.Height == 0);
         Assert(tree.Size == 0);
      }

      public static void Main(string[] args)
      {
         BasicAVLTreeTest();
      }
   }
}
