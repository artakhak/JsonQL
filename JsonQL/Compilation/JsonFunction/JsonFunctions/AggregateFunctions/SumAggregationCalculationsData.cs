namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

public class SumAggregationCalculationsData : AggregationCalculationsData<double>
{
    public double Sum { get; set; } = 0;
    public override double GetResult()
    {
        return Sum;
    }
}