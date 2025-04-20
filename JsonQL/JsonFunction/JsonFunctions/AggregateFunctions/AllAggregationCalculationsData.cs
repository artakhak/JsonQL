namespace JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;

public class AllAggregationCalculationsData : AggregationCalculationsData<bool>
{
    public bool Result { get; set; }

    /// <inheritdoc />
    public override bool GetResult()
    {
        return Result;
    }
}