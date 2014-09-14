using System;

namespace DSA.Tree
{
   /// <summary>
   /// Represents a binary search tree
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IBinarySearchTree<T> where T : IComparable<T>, IEquatable<T>
   {
      /// <summary>
      /// Adds an element T to the binary search tree. Duplicates are not allowed
      /// </summary>
      /// <param name="t">The item to add</param>
      void Add(T t);
      /// <summary>
      /// Gets whether or not the item is in the tree
      /// </summary>
      /// <param name="t">The key to search on</param>
      /// <returns>True if the item is in the tree. False otherwise</returns>
      bool Contains(T t);
      /// <summary>
      /// Removes the key from the tree, if present
      /// </summary>
      /// <param name="t">The item to remove</param>
      /// <returns>Whether or not the tree removed your element</returns>
      bool Remove(T t);
      /// <summary>
      /// Performs a pre-order traversal of the tree, applying the action to each node
      /// </summary>
      /// <param name="act">The action to perform</param>
      void PreOrderTraveral(Action<T> act);
      /// <summary>
      /// Performs an in-order travers of the tree, applying the action to each node
      /// </summary>
      /// <param name="act">The action to apply</param>
      void InOrderTraversal(Action<T> act);
      /// <summary>
      /// Performs a post-order traversal of the tree, applying the action to each node
      /// </summary>
      /// <param name="act">The action to apply</param>
      void PostOrderTraversal(Action<T> act);
   }
}
