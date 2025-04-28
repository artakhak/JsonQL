namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents a class that handles aggregation calculations
/// specifically for boolean results within aggregation functions.
/// </summary>
public class AllAggregationCalculationsData : AggregationCalculationsData<bool>
{
    /// <summary>
    /// Gets or sets the result of the aggregation calculations.
    /// Represents a boolean value indicating the overall outcome
    /// of the aggregation process.
    /// </summary>
    public bool Result { get; set; }

    /// <inheritdoc />
    public override bool GetResult()
    {
        return Result;
    }
}