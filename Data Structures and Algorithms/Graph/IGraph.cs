using System.Collections.Generic;
namespace DSA.Graph
{
   /// <summary>
   /// Represents a Graph
   /// </summary>
   public interface IGraph<T>
   {
      /// <summary>
      /// Gets the graph nodes.
      /// </summary>
      /// <value>
      /// The graph nodes.
      /// </value>
      IEnumerable<IGraphNode<T>> Nodes { get; }
     
      /// <summary>
      /// Gets the root.
      /// </summary>
      /// <value>
      /// The root.
      /// </value>
      IGraphNode<T> Root { get; }
   }
}
