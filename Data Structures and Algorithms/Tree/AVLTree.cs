using System;
using System.Collections;
using System.Collections.Generic;

namespace DSA.Tree
{
   /// <summary>
   /// An implmentation of the AVL tree
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public sealed class AVLTree<T> : IEnumerable<T>, IBinarySearchTree<T> where T : IComparable<T>, IEquatable<T>
   {
      #region private classes
      private sealed class Node
      {
         public Node(T t)
         {
            Value = t;
         }

         public Node Parent { get; set; }

         public Node Left { get; set; }

         public Node Right { get; set; }

         public T Value { get; private set; }

         public override string ToString()
         {
            return string.Format("Node with value {0}", Value);
         }

         public long BalanceFactor
         {
            get
            {
               long leftHeight = Left == null ? 0 : Left.Height;
               long rightHeight = Right == null ? 0 : Right.Height;

               return leftHeight - rightHeight;
            }
         }

         public long Height
         {
            get
            {
               long leftHeight = Left == null ? 0 : Left.Height;
               long rightHeight = Right == null ? 0 : Right.Height;

               return 1 + Math.Max(leftHeight, rightHeight);
            }
         }
      }
      #endregion

      #region private fields
      private Node _root;
      private long _size;
      #endregion

      #region public properties
      /// <summary>
      /// The number of elements in this binary tree
      /// </summary>
      public long Size
      {
         get { return _size; }
      }

      /// <summary>
      /// The current height of the tree
      /// </summary>
      public long Height
      {
         get 
         {
            if (_root == null)
            {
               return 0;
            }

            return _root.Height; 
         }
      }
      #endregion

      #region public methods

      /// <summary>
      /// Returns an enumerator that iterates through the collection.
      /// </summary>
      /// <returns>
      /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
      /// </returns>
      public IEnumerator<T> GetEnumerator()
      {
         if (_root == null)
         {
            yield break;
         }

         var list = new List<T>();
         InOrderTraversal(list.Add);

         foreach (var i in list)
         {
            yield return i;
         }
      }

      /// <summary>
      /// Returns an enumerator that iterates through a collection.
      /// </summary>
      /// <returns>
      /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
      /// </returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <summary>
      /// Returns a <see cref="System.String" /> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String" /> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         return string.Format("Binary Search Tree with {0} as Root", _root);
      }

      #region Implementation of IBinarySearchTree<T>

      /// <summary>
      /// Adds an element T to the binary search tree. Duplicates are not allowed
      /// </summary>
      /// <param name="t">The item to add</param>
      public void Add(T t)
      {
         if (_root == null)
         {
            _size++;
            _root = new Node(t);
            return;
         }

         Node result, wouldBeParent;
         RetrieveWithParent(_root, null, t, out result, out wouldBeParent);

         if (result == null && wouldBeParent == null)
         {
            throw new InvalidOperationException("Invalid state detected within the binary tree. Unable to get node or parent and the focus is somehow not null");
         }

         if (result != null)
         {
            return;
         }

         var freshNode = new Node(t);
         var keyComparedToMe = t.CompareTo(wouldBeParent.Value);

         if (keyComparedToMe < 0)
         {
            CreateLinkLeft(wouldBeParent, freshNode);
         }
         else if (keyComparedToMe > 0)
         {
            CreateLinkRight(wouldBeParent, freshNode);
         }

         _size++;

         RebalanceTree(wouldBeParent);
      }

      /// <summary>
      /// Gets whether or not the item is in the tree
      /// </summary>
      /// <param name="t">The key to search on</param>
      /// <returns>True if the item is in the tree. False otherwise</returns>
      public bool Contains(T t)
      {
         Node result;
         RetrieveWithoutParent(_root, t, out result);

         return result != null;
      }

      /// <summary>
      /// Removes the key from the tree, if present
      /// </summary>
      /// <param name="t">The item to remove</param>
      public bool Remove(T t)
      {
         Node focus;
         RetrieveWithoutParent(_root, t, out focus);

         if (focus == null)
         {
            return false;
         }

         RemoveHelper(focus);
         _size--;

         return true;
      }

      /// <summary>
      /// Performs a pre-order traversal of the tree, applying the action to each node
      /// </summary>
      /// <param name="act">The action to perform</param>
      public void PreOrderTraveral(Action<T> act)
      {
         PreOrderHelper(_root, act);
      }

      /// <summary>
      /// Performs an in-order travers of the tree, applying the action to each node
      /// </summary>
      /// <param name="act">The action to apply</param>
      public void InOrderTraversal(Action<T> act)
      {
         InOrderHelper(_root, act);
      }

      /// <summary>
      /// Performs a post-order traversal of the tree, applying the action to each node
      /// </summary>
      /// <param name="act">The action to apply</param>
      public void PostOrderTraversal(Action<T> act)
      {
         PostOrderHelper(_root, act);
      }

      #endregion
      #endregion

      #region private methods
      private void RemoveHelper(Node focus)
      {
         /*
          * 3 cases here:
          * 1. Node has no children
          * 2. Node has 1 child
          * 3. Node has 2 children
          */
         int numChildren = 0;
         if (focus.Left != null)
         {
            numChildren++;
         }

         if (focus.Right != null)
         {
            numChildren++;
         }

         if (numChildren == 0)
         {
            SeverParentConnection(focus);
            
            if(ReferenceEquals(_root, focus))
            {
               _root = null;
               return;
            }

            if (focus.Parent != null)
            {
               RebalanceTree(focus.Parent);
            }
         }
         else if (numChildren == 1)
         {
            var childNode = focus.Left != null ? focus.Left : focus.Right;
            SeverParentConnection(childNode);
            ConnectParentWithChild(focus, childNode);

            if (ReferenceEquals(_root, focus))
            {
               _root = childNode;
            }
         }
         else
         {
            var replacement = GetReplacementCandidate(focus);
            var clone = new Node(replacement.Value);

            //Hook up the clone 
            CreateLinkLeft(clone, focus.Left);
            CreateLinkRight(clone, focus.Right);
            ConnectParentWithChild(focus, clone);

            //Set the root equal to the clone if we have to
            if(ReferenceEquals(_root, focus))
            {
               _root = clone;
            }

            //Delete the original replacement
            RemoveHelper(replacement);
         }
      }

      private static void CreateLinkCore(Node parent, Node child, bool assignLeft)
      {
         if (parent == null || ReferenceEquals(parent, child))
         {
            return;
         }

         if (assignLeft)
         {
            parent.Left = child;
         }
         else
         {
            parent.Right = child;
         }

         if (child != null)
         {
            child.Parent = parent;
         }
      }

      private static void CreateLinkRight(Node parent, Node child)
      {
         CreateLinkCore(parent, child, false);
      }

      private static void CreateLinkLeft(Node parent, Node child)
      {
         CreateLinkCore(parent, child, true);
      }

      private static void SeverParentConnection(Node child)
      {
         if (child == null)
         {
            return;
         }

         var parent = child.Parent;
         if (parent != null)
         {
            if (ReferenceEquals(parent.Left, child))
            {
               parent.Left = null;
            }
            else
            {
               parent.Right = null;
            }
         }

         child.Parent = null;
      }

      private static void ConnectParentWithChild(Node reference, Node child)
      {
         if (reference == null)
         {
            return;
         }

         var parent = reference.Parent;

         if (parent == null)
         {
            if (child != null)
            {
               child.Parent = null;
            }
         }
         else
         {
            if (ReferenceEquals(parent.Left, reference))
            {
               CreateLinkLeft(parent, child);
            }
            else
            {
               CreateLinkRight(parent, child);
            }
         }
      }

      private static Node GetReplacementCandidate(Node focus)
      {
         if (focus == null)
         {
            return null;
         }

         //Go with next smallest first
         if (focus.Left != null)
         {
            Node chosenNode = focus.Left;
            while (chosenNode.Right != null)
            {
               chosenNode = chosenNode.Right;
            }

            return chosenNode;
         }
         
         //Go with next largest second
         if (focus.Right != null)
         {
            Node chosenNode = focus.Right;
            while (chosenNode.Left != null)
            {
               chosenNode = chosenNode.Left;
            }

            return chosenNode;
         }

         //We're at the focus
         return null;
      }

      private static void PreOrderHelper(Node focus, Action<T> act)
      {
         if (focus == null)
         {
            return;
         }

         act.Invoke(focus.Value);
         PreOrderHelper(focus.Left, act);
         PreOrderHelper(focus.Right, act);
      }

      private static void InOrderHelper(Node focus, Action<T> act)
      {
         if (focus == null)
         {
            return;
         }

         InOrderHelper(focus.Left, act);
         act.Invoke(focus.Value);
         InOrderHelper(focus.Right, act);
      }

      private static void PostOrderHelper(Node focus, Action<T> act)
      {
         if (focus == null)
         {
            return;
         }

         PostOrderHelper(focus.Left, act);
         PostOrderHelper(focus.Right, act);
         act.Invoke(focus.Value);
      }

      private static void RotateLeft(Node root)
      {
         //Can't rotate left if the current node or its right child is null
         if (root == null || root.Right == null)
         {
            return;
         }
         
         var rightChild = root.Right;

         ConnectParentWithChild(root, rightChild);
         CreateLinkRight(root, rightChild.Left);
         CreateLinkLeft(rightChild, root);
      }

      private static void RotateRight(Node root)
      {
         //Can't rotate right if the current node or its left child are null
         if (root == null || root.Left == null)
         {
            return;
         }

         var leftChild = root.Left;

         ConnectParentWithChild(root, leftChild);
         CreateLinkLeft(root, leftChild.Right);
         CreateLinkRight(leftChild, root);
      }

      private void RebalanceTree(Node focus)
      {
         if (focus == null)
         {
            return;
         }

         var previousRoot = _root;
         var parentNode = focus.Parent;
         var balanceFactor = focus.BalanceFactor;
         var didRebalance = false;

         if (balanceFactor < -1)
         {
            didRebalance = true;

            //Right side is heavy
            if (focus.Right.BalanceFactor > 0)
            {
               //Right-Left case
               RotateRight(focus.Right);
            }

            //Right-Right case
            RotateLeft(focus);
         }
         else if (balanceFactor > 1)
         {
            didRebalance = true;

            //Left side is heavy
            if (focus.Left.BalanceFactor < 0)
            {
               //Left-Right case
               RotateLeft(focus.Left);
            }

            //Left-Left case
            RotateRight(focus);
         }

         if(ReferenceEquals(previousRoot, focus) && didRebalance)
         {
            _root = focus.Parent;
         }
         else
         {
            RebalanceTree(parentNode);
         }
      }

      private static void RetrieveWithoutParent(Node root, T key, out Node result)
      {
         Node _;
         RetrieveWithParent(root, null, key, out result, out _);
      }

      private static void RetrieveWithParent(Node root, Node parent, T key, out Node result, out Node parentNode)
      {
         result = null;

         if (root == null)
         {
            parentNode = parent;
            return;
         }

         int keyComparedToMe = key.CompareTo(root.Value);
         if (keyComparedToMe < 0)
         {
            RetrieveWithParent(root.Left, root, key, out result, out parentNode);
            return;
         }

         if (keyComparedToMe > 0)
         {
            RetrieveWithParent(root.Right, root, key, out result, out parentNode);
            return;
         }

         result = root;
         parentNode = parent;
      }
      #endregion
   }
}
