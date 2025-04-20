namespace JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;

public class MinMaxAggregationCalculationsData : AggregationCalculationsData<double>
{
    public double MinMaxValue { get; set; }
    public override double GetResult()
    {
        return MinMaxValue;
    }
}