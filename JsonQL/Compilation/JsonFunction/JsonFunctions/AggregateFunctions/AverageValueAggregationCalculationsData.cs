// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents the data required for performing aggregation calculations specific to
/// averaging a set of values. This class specializes in handling computations for
/// aggregate operations involving double-type data values.
/// </summary>
/// <remarks>
/// This class maintains the state for average computation by tracking the current sum
/// of evaluated values and their count. It provides the utility to compute the final
/// average result based on these tracked values.
/// </remarks>
public class AverageValueAggregationCalculationsData : AggregationCalculationsData<double>
{
    /// <summary>
    /// Gets or sets the current aggregated value for the averaging calculation.
    /// </summary>
    /// <remarks>
    /// This property holds the running total or sum of double values that have been evaluated
    /// during an average aggregation process. It is incremented as new values are evaluated,
    /// and its final state is used to compute the average result.
    /// </remarks>
    public double CurrentValue { get; set; }

    /// <summary>
    /// Gets or sets the count of values that have been evaluated during the averaging calculation.
    /// </summary>
    /// <remarks>
    /// This property tracks the total number of double values processed and included in the calculation
    /// for computing the average result. It is incremented as each valid value is evaluated.
    /// </remarks>
    public int NumberOfEvaluatedValues { get; set; }

    /// <inheritdoc />
    public override double GetResult()
    {
        if (NumberOfEvaluatedValues == 0)
            return 0;

        return CurrentValue / NumberOfEvaluatedValues;
    }
}