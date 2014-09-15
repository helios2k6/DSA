using System;
namespace DSA.Graph
{
   /// <summary>
   /// Represents a graph edge
   /// </summary>
   public class GraphEdge<T> : IGraphEdge<T> where T : IEquatable<T>
   {
      public GraphEdge(IGraphNode<T> start, IGraphNode<T> end, int weight, bool isDirected)
      {
         Start = start;
         End = end;
         Weight = weight;
         IsDirected = isDirected;
      }

      /// <summary>
      /// Gets the start node.
      /// </summary>
      /// <value>
      /// The start node.
      /// </value>
      public IGraphNode<T> Start { get; private set; }

      /// <summary>
      /// Gets the end node.
      /// </summary>
      /// <value>
      /// The end node.
      /// </value>
      public IGraphNode<T> End { get; private set; }

      /// <summary>
      /// Gets the weight of the edge.
      /// </summary>
      /// <value>
      /// The weight of the edge.
      /// </value>
      public int Weight { get; private set; }

      /// <summary>
      /// Gets a value indicating whether this edge is directed.
      /// </summary>
      /// <value>
      /// <c>true</c> if this edge is directed; otherwise, <c>false</c>.
      /// </value>
      public bool IsDirected { get; private set; }

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
      public bool Equals(IGraphEdge<T> other)
      {
         if (EqualsPreamble(other) == false) return false;

         return Start.Equals(other.Start)
            && End.Equals(other.End)
            && Weight == other.Weight
            && IsDirected == other.IsDirected;
      }

      /// <summary>
      /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
      /// </summary>
      /// <param name="other">The <see cref="System.Object" /> to compare with this instance.</param>
      /// <returns>
      ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
      /// </returns>
      public override bool Equals(object other)
      {
         if (EqualsPreamble(other) == false) return false;

         return Equals(other as IGraphEdge<T>);
      }

      /// <summary>
      /// Returns a hash code for this instance.
      /// </summary>
      /// <returns>
      /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
      /// </returns>
      public override int GetHashCode()
      {
         return Start.GetHashCode() ^ End.GetHashCode() ^ Weight.GetHashCode() ^ IsDirected.GetHashCode();
      }

      /// <summary>
      /// Returns a <see cref="System.String" /> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String" /> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         return string.Format("Graph Edge [Start:{0}] | [End:{1}] | [Weight:{2}] | [Directed:{3}]", Start, End, Weight, IsDirected);
      }
   }
}
