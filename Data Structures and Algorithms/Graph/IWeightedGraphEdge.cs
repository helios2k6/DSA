namespace DSA.Graph
{
   /// <summary>
   /// Represents a weighted edge in a graph
   /// </summary>
   /// <typeparam name="T">The type associated with the graph node</typeparam>
   /// <typeparam name="TWeight">The type of the weight.</typeparam>
   public interface IWeightedGraphEdge<T, TWeight> : IGraphEdge<T>
   {
      /// <summary>
      /// Gets the weight.
      /// </summary>
      /// <value>
      /// The weight.
      /// </value>
      TWeight Weight { get; }
   }
}
