namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents the base class for defining aggregation calculation behaviors in data processing.
/// </summary>
/// <typeparam name="TResult">The type of the result produced by the aggregation calculation, constrained to types implementing <see cref="IComparable"/>.</typeparam>
public abstract class AggregationCalculationsData<TResult> where TResult : IComparable?
{
    /// <summary>
    /// Calculates and returns the resulting value based on the aggregation logic implemented in the derived class.
    /// </summary>
    /// <returns>The calculated result of the specified type based on the aggregation logic.</returns>
    public abstract TResult GetResult();
}