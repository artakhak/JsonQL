namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

public class AverageValueAggregationCalculationsData : AggregationCalculationsData<double>
{
    public double CurrentValue { get; set; } = 0;
    public int NumberOfEvaluatedValues { get; set; }
    public override double GetResult()
    {
        if (NumberOfEvaluatedValues == 0)
            return 0;

        return CurrentValue / NumberOfEvaluatedValues;
    }
}