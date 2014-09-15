using System;
namespace DSA.Graph
{
   /// <summary>
   /// Represents a ndoe in a graph.
   /// </summary>
   /// <typeparam name="T">The type of the value in the graph node.</typeparam>
   public class GraphNode<T> : IGraphNode<T> where T : IEquatable<T>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="GraphNode{T}"/> class.
      /// </summary>
      /// <param name="value">The value.</param>
      public GraphNode(T value)
      {
         Value = value;
      }

      /// <summary>
      /// Gets the value.
      /// </summary>
      /// <value>
      /// The value.
      /// </value>
      /// <exception cref="System.NotImplementedException"></exception>
      public T Value { get; private set; }

      /// <summary>
      /// Indicates whether the current object is equal to another object of the same type.
      /// </summary>
      /// <param name="other">An object to compare with this object.</param>
      /// <returns>
      /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
      /// </returns>
      /// <exception cref="System.NotImplementedException"></exception>
      public bool Equals(IGraphNode<T> other)
      {
         if(EqualsPreamble(other))
         {
            return Value.Equals(other.Value);
         }

         return false;
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
         if(EqualsPreamble(obj))
         {
            return Equals(obj as IGraphNode<T>);
         }

         return false;
      }

      /// <summary>
      /// Returns a hash code for this instance.
      /// </summary>
      /// <returns>
      /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
      /// </returns>
      public override int GetHashCode()
      {
         return Value.GetHashCode();
      }

      /// <summary>
      /// Returns a <see cref="System.String" /> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String" /> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         return string.Format("Graph Node with {0}", Value);
      }

      private bool EqualsPreamble(object other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         if (GetType() != other.GetType()) return false;

         return true;
      }
   }
}
