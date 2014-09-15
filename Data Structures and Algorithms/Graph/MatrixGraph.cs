using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DSA.Graph
{
   /// <summary>
   /// Represents a graph using a boolean 2D matrix
   /// </summary>
   /// <typeparam name="T">The type stored in the graph nodes</typeparam>
   public sealed class MatrixGraph<T> : GraphBase<T> where T : IEquatable<T>
   {
      private readonly ISet<IGraphNode<T>> _nodes;
      private readonly int?[][] _edgeMatrix;
      private readonly IDictionary<IGraphNode<T>, int> _nodeIndexMap;
      private readonly IDictionary<int, IGraphNode<T>> _indexToNodeMap;

      /// <summary>
      /// Initializes a new instance of the <see cref="MatrixGraph{T}"/> class.
      /// </summary>
      /// <param name="nodes">The nodes.</param>
      /// <param name="edges">The edges.</param>
      public MatrixGraph(IEnumerable<IGraphNode<T>> nodes, IEnumerable<IGraphEdge<T>> edges)
      {
         _nodes = new HashSet<IGraphNode<T>>(nodes);
         var indexMaps = InitializeIndexMaps(edges);
         _nodeIndexMap = indexMaps.Item1;
         _indexToNodeMap = indexMaps.Item2;
         _edgeMatrix = CreateEdgeMatrixMatrix(_nodes.Count, _nodes.Count);
         InitializeEdgeMatrix(_edgeMatrix, _nodeIndexMap, edges);
      }


      /// <summary>
      /// Initializes a new instance of the <see cref="MatrixGraph{T}"/> class.
      /// </summary>
      /// <param name="edges">The edges of the graph.</param>
      public MatrixGraph(IEnumerable<IGraphEdge<T>> edges)
         : this(edges.Select(t => t.Start), edges)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="MatrixGraph{T}"/> class.
      /// </summary>
      public MatrixGraph()
      {
         _nodes = new HashSet<IGraphNode<T>>();
         _edgeMatrix = CreateEdgeMatrixMatrix(0, 0);
         _nodeIndexMap = new Dictionary<IGraphNode<T>, int>();
      }

      /// <summary>
      /// Gets the nodes.
      /// </summary>
      /// <value>
      /// The nodes.
      /// </value>
      public override IEnumerable<IGraphNode<T>> Nodes
      {
         get { return _nodes; }
      }

      /// <summary>
      /// Gets the neighbors of a given node.
      /// </summary>
      /// <param name="startNode">The start node to find the neighbors from.</param>
      /// <returns>An enumerable of neighbors</returns>
      public override IEnumerable<IGraphEdge<T>> GetNeighbors(IGraphNode<T> startNode)
      {
         int startEdge;
         if (_nodeIndexMap.TryGetValue(startNode, out startEdge))
         {
            var edgeList = new List<IGraphEdge<T>>();
            int?[] edges = _edgeMatrix[startEdge];

            for (int endEdge = 0; endEdge != edges.Length; endEdge++)
            {
               int? candidateEdgeWeight = edges[endEdge];

               if (candidateEdgeWeight != null)
               {
                  var endNode = _indexToNodeMap[endEdge];

                  var graphEdge = new GraphEdge<T>(startNode, endNode, candidateEdgeWeight.Value, IsNodeDirected(_edgeMatrix, startEdge, endEdge));
               }
            }
            return edgeList;
         }

         return Enumerable.Empty<IGraphEdge<T>>();
      }

      protected override string GraphType
      {
         get { return "Matrix Graph"; }
      }

      private static bool IsNodeDirected(int?[][] edgeMatrix, int startNode, int endNode)
      {
         int?[] startNodeEdges = edgeMatrix[startNode];
         int?[] endNodeEdges = edgeMatrix[endNode];

         int? weightFromStartToEnd = startNodeEdges[endNode];
         int? weightFromEndToStart = startNodeEdges[startNode];

         bool isUndirected = weightFromStartToEnd.HasValue && weightFromEndToStart.HasValue;
         return !isUndirected;
      }

      private static void InitializeEdgeMatrix(int?[][] edgeMatrix, IDictionary<IGraphNode<T>, int> indexMap, IEnumerable<IGraphEdge<T>> edges)
      {
         foreach (var edge in edges)
         {
            int edgeStartNode = indexMap[edge.Start];
            int edgeEndNode = indexMap[edge.End];

            edgeMatrix[edgeStartNode][edgeEndNode] = edge.Weight;

            if (edge.IsDirected == false)
            {
               edgeMatrix[edgeEndNode][edgeStartNode] = edge.Weight;
            }
         }
      }

      private static Tuple<IDictionary<IGraphNode<T>, int>, IDictionary<int, IGraphNode<T>>> InitializeIndexMaps(IEnumerable<IGraphEdge<T>> edges)
      {
         IDictionary<IGraphNode<T>, int> nodeToIndexMap = new Dictionary<IGraphNode<T>, int>();
         IDictionary<int, IGraphNode<T>> indexToNodeMap = new Dictionary<int, IGraphNode<T>>();

         int waterMark = 0;
         foreach (var edge in edges)
         {
            var startNode = edge.Start;
            int currentIndex;
            if (nodeToIndexMap.TryGetValue(startNode, out currentIndex) == false)
            {
               currentIndex = waterMark;
               waterMark++;

               nodeToIndexMap[startNode] = currentIndex;
               indexToNodeMap[currentIndex] = startNode;
            }
         }

         return Tuple.Create(nodeToIndexMap, indexToNodeMap);
      }

      private static int?[][] CreateEdgeMatrixMatrix(int rows, int cols)
      {
         var matrix = new int?[rows][];

         for (int i = 0; i < rows; i++)
         {
            matrix[i] = new int?[cols];
         }

         return matrix;
      }
   }
}

