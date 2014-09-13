using System;
using System.Collections;
using DSA.Graph;
using System.Collections.Generic;
using System.Linq;

namespace DSA
{
   public sealed class DijkstrasAlgorithm
   {
      private static readonly DijkstrasAlgorithm InstanceInternal = new DijkstrasAlgorithm();

      public static DijkstrasAlgorithm Instance { get { return InstanceInternal; } }

      public IDictionary<IGraphNode<T>, int> Execute<T>(IGraph<T> graph, IGraphNode<T> initialNode)
      {
         var tenativeDistances = new Dictionary<IGraphNode<T>, int>();
         tenativeDistances.Add(initialNode, 0);

         var unvisitedNodes = new HashSet<IGraphNode<T>>(graph.Nodes);
         unvisitedNodes.Remove(initialNode);

         var currentNode = initialNode;

         while (unvisitedNodes.Any())
         {
            var currentDistance = tenativeDistances[currentNode];

            //Cycle through all neighbors
            var neighbors = graph.GetNeighbors(currentNode);
            foreach (var neighbor in neighbors.Where(t => unvisitedNodes.Contains(t.End)))
            {
               int tenativeDistance;
               if (tenativeDistances.TryGetValue(neighbor.End, out tenativeDistance) == false)
               {
                  tenativeDistances[currentNode] = currentDistance + neighbor.Weight;
               }
               else
               {
                  var comparisonDistance = currentDistance + neighbor.Weight;

                  if (comparisonDistance < tenativeDistance)
                  {
                     tenativeDistances[currentNode] = comparisonDistance;
                  }
               }
             
            }
            unvisitedNodes.Remove(currentNode);
            currentNode = unvisitedNodes.Take(1).SingleOrDefault();
         }

         return tenativeDistances;
      }
   }
}

