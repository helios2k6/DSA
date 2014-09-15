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
      private static void TestBasicGraphActions<T>(IGraph<T> graph) where T : IEquatable<T>
      {
         
      }

      /// <summary>
      /// Tests the matrix graph.
      /// </summary>
      [TestMethod]
      public void TestMatrixGraph()
      {
         var emptyGraph = new MatrixGraph<int>();
         

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
