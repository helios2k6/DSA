using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSA.Graph
{
   /// <summary>
   /// The base class for all IGraph{T} implementations. It provides all Equality facilities
   /// </summary>
   /// <typeparam name="T">The type of the value stored in the graph nodes</typeparam>
   public abstract class GraphBase<T> : IGraph<T> where T : IEquatable<T>
   {
      private bool EqualsPreamble(object other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         if (GetType() != other.GetType()) return false;

         return true;
      }

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <param name="other">An object to compare with this object.</param>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      public bool Equals(IGraph<T> other)
      {
         if (EqualsPreamble(other) == false) return false;

         return Enumerable.SequenceEqual(Nodes, other.Nodes);
      }

      /// <summary>
      /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
      /// </summary>
      /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
      /// <returns>
      ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
      /// </returns>
      public override bool Equals(object obj)
      {
         if (EqualsPreamble(obj) == false) return false;

         return Equals(obj as IGraph<T>);
      }

      /// <summary>
      /// Returns a hash code for this instance.
      /// </summary>
      /// <returns>
      /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
      /// </returns>
      public override int GetHashCode()
      {
         return Nodes.Aggregate(0, (state, node) => state ^ node.GetHashCode());
      }

      /// <summary>
      /// Returns a <see cref="System.String" /> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String" /> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         var builder = new StringBuilder();

         builder.AppendLine(GraphType).AppendLine("Nodes").AppendLine("{");
         foreach (var node in Nodes)
         {
            builder.AppendLine(node.ToString());
         }

         builder.AppendLine("}");
         return builder.ToString();
      }

      /// <summary>
      /// Gets the type of the graph.
      /// </summary>
      /// <value>
      /// The type of the graph.
      /// </value>
      protected abstract string GraphType { get; }

      /// <summary>
      /// Gets the nodes.
      /// </summary>
      /// <value>
      /// The nodes.
      /// </value>
      public abstract IEnumerable<IGraphNode<T>> Nodes { get; }

      /// <summary>
      /// Gets the neighbors of the given node.
      /// </summary>
      /// <param name="node">The node to find the neighbors of.</param>
      /// <returns>An enumerable of neighbors</returns>
      public abstract IEnumerable<IGraphEdge<T>> GetNeighbors(IGraphNode<T> node);
   }
}
