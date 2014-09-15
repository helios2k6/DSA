using System;
using System.Collections.Generic;
using System.Linq;

namespace DSA.Graph
{
   /// <summary>
   /// IGraph, IGraphNode, and IGraphEdge extensions
   /// </summary>
   public static class IGraphExtensions
   {
      /// <summary>
      /// Makes the edges.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="this">The this.</param>
      /// <param name="neighbors">The neighbors.</param>
      /// <returns></returns>
      public static IEnumerable<IGraphEdge<T>> MakeEdges<T>(this IGraphNode<T> @this, params IGraphNode<T>[] neighbors) where T : IEquatable<T>
      {
         return neighbors.Select(neighbor => new GraphEdge<T>(@this, neighbor, 0, false));
      }
   }
}
