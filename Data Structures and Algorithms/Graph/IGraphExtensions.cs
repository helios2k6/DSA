using System;
using System.Collections.Generic;
using System.Linq;

namespace DSA.Graph
{
   public static class IGraphExtensions
   {
      public static IEnumerable<IGraphEdge<T>> MakeEdges<T>(this IGraphNode<T> @this, params IGraphNode<T>[] neighbors) where T : IEquatable<T>
      {
         return neighbors.Select(neighbor => new GraphEdge<T>(@this, neighbor, 0, false));
      }


   }
}
