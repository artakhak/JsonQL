namespace JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;

public abstract class AggregationCalculationsData<TResult> where TResult : IComparable?
{
    public abstract TResult GetResult();
}