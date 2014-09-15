using System;

namespace DSA.Graph
{
   /// <summary>
   /// Represents an edge in a graph.
   /// </summary>
   /// <typeparam name="T">The type associated with the graph nodes.</typeparam>
   public interface IGraphEdge<T> : IEquatable<IGraphEdge<T>> where T : IEquatable<T>
   {
      /// <summary>
      /// Gets the start of the graph edge.
      /// </summary>
      /// <value>
      /// The start.
      /// </value>
      IGraphNode<T> Start { get; }
      
      /// <summary>
      /// Gets the end of the graph edge.
      /// </summary>
      /// <value>
      /// The end.
      /// </value>
      IGraphNode<T> End { get; }

      /// <summary>
      /// Gets the weight.
      /// </summary>
      /// <value>The weight.</value>
      int Weight { get; }
     
      /// <summary>
      /// Gets a value indicating whether this graph edge is directed.
      /// </summary>
      /// <value>
      /// <c>true</c> if this graph edge is directed; otherwise, <c>false</c>.
      /// </value>
      bool IsDirected { get; }
   }
}
