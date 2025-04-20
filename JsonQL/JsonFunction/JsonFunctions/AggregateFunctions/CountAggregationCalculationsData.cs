namespace JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;

public class CountAggregationCalculationsData : AggregationCalculationsData<double>
{
    public int NumberOfEvaluatedValues { get; set; }

    /// <inheritdoc />
    public override double GetResult()
    {
        return NumberOfEvaluatedValues;
    }
}