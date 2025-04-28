namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents the data structure used for count-based aggregation calculations.
/// This class specifically extends the base `AggregationCalculationsData` class
/// with type `double`, allowing aggregation of numeric values.
/// </summary>
public class CountAggregationCalculationsData : AggregationCalculationsData<double>
{
    /// <summary>
    /// Gets or sets the number of values that have been evaluated during the aggregation process.
    /// This property is primarily used in count-based aggregation calculations to track the total
    /// count of values considered, either based on a predicate or directly from the input collection.
    /// </summary>
    public int NumberOfEvaluatedValues { get; set; }

    /// <inheritdoc />
    public override double GetResult()
    {
        return NumberOfEvaluatedValues;
    }
}