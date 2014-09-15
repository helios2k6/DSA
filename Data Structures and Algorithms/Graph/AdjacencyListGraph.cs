using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSA.Graph
{
   /// <summary>
   /// Represents a graph using an adjacency list for edge management.
   /// </summary>
   /// <typeparam name="T">The type stored within the graph nodes.</typeparam>
   public sealed class AdjacencyListGraph<T> : GraphBase<T> where T : IEquatable<T>
   {
      private readonly ISet<IGraphNode<T>> _nodes;
      private readonly IDictionary<IGraphNode<T>, ICollection<IGraphEdge<T>>> _edges;

      /// <summary>
      /// Initializes a new instance of the <see cref="AdjacencyListGraph{T}"/> class.
      /// </summary>
      /// <param name="nodes">The nodes.</param>
      /// <param name="edges">The edges.</param>
      public AdjacencyListGraph(IEnumerable<IGraphNode<T>> nodes, IEnumerable<IGraphEdge<T>> edges)
      {
         _nodes = new HashSet<IGraphNode<T>>(nodes);
         _edges = new Dictionary<IGraphNode<T>, ICollection<IGraphEdge<T>>>();

         foreach (var edge in edges)
         {
            ProcessEdge(edge);
         }
      }

      private void ProcessEdge(IGraphEdge<T> edge)
      {
         ICollection<IGraphEdge<T>> collection;
         if (_edges.TryGetValue(edge.Start, out collection) == false)
         {
            collection = new HashSet<IGraphEdge<T>>();
            _edges[edge.Start] = collection;
         }

         collection.Add(edge);

         if (edge.IsDirected == false)
         {
            var reverseEdge = new GraphEdge<T>(edge.End, edge.Start, edge.Weight, false);
            collection.Add(reverseEdge);
         }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AdjacencyListGraph{T}"/> class.
      /// </summary>
      /// <param name="edges">The edges of the graph.</param>
      public AdjacencyListGraph(IEnumerable<IGraphEdge<T>> edges)
         : this(edges.Select(t => t.Start), edges)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="AdjacencyListGraph{T}"/> class.
      /// </summary>
      public AdjacencyListGraph()
      {
         _nodes = new HashSet<IGraphNode<T>>();
         _edges = new Dictionary<IGraphNode<T>, ICollection<IGraphEdge<T>>>();
      }

      /// <summary>
      /// Gets the neighbors.
      /// </summary>
      /// <param name="node">The node to get the neighbors of.</param>
      /// <returns>An enumerable of neighbors.</returns>
      public override IEnumerable<IGraphEdge<T>> GetNeighbors(IGraphNode<T> node)
      {
         ICollection<IGraphEdge<T>> collection;
         if (_edges.TryGetValue(node, out collection))
         {
            return collection;
         }

         return Enumerable.Empty<IGraphEdge<T>>();
      }

      /// <summary>
      /// Gets the nodes of this graph.
      /// </summary>
      /// <value>
      /// The nodes.
      /// </value>
      public override IEnumerable<IGraphNode<T>> Nodes
      {
         get { return _nodes; }
      }

      protected override string GraphType
      {
         get { return "Adjacency List Graph"; }
      }
   }
}

