// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents data used for performing Min and Max aggregation calculations.
/// Derived from <see cref="AggregationCalculationsData{TResult}"/> where TResult is <see cref="double"/>.
/// </summary>
public class MinMaxAggregationCalculationsData : AggregationCalculationsData<double>
{
    /// <summary>
    /// Gets or sets the aggregated value used for Min and Max calculations.
    /// This property holds the current minimum or maximum value within the aggregation process.
    /// </summary>
    public double MinMaxValue { get; set; }

    /// <inheritdoc />
    public override double GetResult()
    {
        return MinMaxValue;
    }
}
