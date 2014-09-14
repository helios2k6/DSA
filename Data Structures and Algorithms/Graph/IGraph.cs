using System.Collections.Generic;
using System;


namespace DSA.Graph
{
   /// <summary>
   /// Represents a Graph
   /// </summary>
   public interface IGraph<T> : IEquatable<IGraph<T>>
   {
      /// <summary>
      /// Gets the graph nodes.
      /// </summary>
      /// <value>
      /// The graph nodes.
      /// </value>
      IEnumerable<IGraphNode<T>> Nodes { get; }

      /// <summary>
      /// Gets the neighbors of a given node.
      /// </summary>
      /// <returns>The neighbors of a given node.</returns>
      /// <param name="node">The node to get the neighbors of</param>
      IEnumerable<IGraphEdge<T>> GetNeighbors(IGraphNode<T> node);
   }
}
