using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DSA.Graph;

namespace Tests
{
   /// <summary>
   /// The graph tests
   /// </summary>
   [TestClass]
   public class GraphTests
   {
      /// <summary>
      /// Tests the matrix graph.
      /// </summary>
      [TestMethod]
      public void TestMatrixGraph()
      {
         var emptyGraph = new MatrixGraph<int>();

         var root = new GraphNode<int>(10);
         var expandedGraph = root.MakeEdges<int>(null);
      }

      /// <summary>
      /// Tests the adjacency list graph.
      /// </summary>
      [TestMethod]
      public void TestAdjacencyListGraph()
      {

      }
   }
}
