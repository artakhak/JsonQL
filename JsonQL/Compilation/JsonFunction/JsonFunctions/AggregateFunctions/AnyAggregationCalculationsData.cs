namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

public class AnyAggregationCalculationsData : AggregationCalculationsData<bool>
{
    public bool Result { get; set; } 

    /// <inheritdoc />
    public override bool GetResult()
    {
        return Result;
    }
}