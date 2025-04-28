namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents the data required for performing sum aggregation calculations.
/// This class is a specialized implementation for handling aggregation operations
/// that result in summing up numerical values.
/// </summary>
public class SumAggregationCalculationsData : AggregationCalculationsData<double>
{
    /// <summary>
    /// Gets or sets the sum value being aggregated during a sum operation.
    /// This property accumulates the total of numerical values as they are processed.
    /// </summary>
    public double Sum { get; set; }

    /// <inheritdoc />
    public override double GetResult()
    {
        return Sum;
    }
}