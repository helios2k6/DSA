using System.Collections.Generic;
using System;


namespace DSA.Graph
{
   /// <summary>
   /// Represents a node on a graph
   /// </summary>
   /// <typeparam name="T">The type of value stored in the node</typeparam>
   /// <typeparam name="TWeight>The type of the weight</typeparam>
   public interface IGraphNode<T>
   {
      /// <summary>
      /// Gets the value.
      /// </summary>
      /// <value>
      /// The value.
      /// </value>
      T Value { get; }

      /// <summary>
      /// Gets the edges incident to this graph node
      /// </summary>
      /// <value>
      /// The edges.
      /// </value>
      IEnumerable<IGraphEdge<T>> Edges { get; }
   }
}
