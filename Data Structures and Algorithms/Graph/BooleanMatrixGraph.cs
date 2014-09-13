using System;
using DSA.Graph;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DSA
{
   public sealed class BooleanMatrixGraph<T> : IGraph<T>
   {
      private readonly IDictionary<IDictionary<IGraphNode<T>, IGraphNode<T>>, bool> _neighborMatrix;

      public BooleanMatrixGraph()
      {
         _neighborMatrix = new Dictionary<IDictionary<IGraphNode<T>, IGraphNode<T>>, bool>();
      }

      #region IGraph implementation

      public IEnumerable<IGraphEdge<T>> GetNeighbors(IGraphNode<T> node)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<IGraphNode<T>> Nodes
      {
         get
         {
            return from kvp in _neighborMatrix
                   from map in kvp.Key
                   select map.Key;
         }
      }

      #endregion
   }
}

