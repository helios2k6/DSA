using System;
using DSA.Graph;
using System.Collections.Generic;

namespace DSA
{
   public sealed class PointerGraph<T> : IGraph<T>
   {
      private readonly ISet<IGraphNode<T>> _nodes;

      public PointerGraph(IEnumerable<IGraphNode<T>> nodes)
      {
         _nodes = new HashSet<IGraphNode<T>>(nodes);
      }

      public PointerGraph()
      {
         _nodes = new HashSet<IGraphNode<T>>();
      }

      #region IGraph implementation
      public IEnumerable<IGraphEdge<T>> GetNeighbors(IGraphNode<T> node)
      {
         return node.Edges;
      }

      public IEnumerable<IGraphNode<T>> Nodes { get; private set; }
      #endregion
   }
}

