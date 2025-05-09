// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents a specific type of aggregation calculation data used to determine any matching condition
/// within a data collection. The class is designed to handle boolean results for aggregation expressions
/// and provides methods to retrieve the evaluated result.
/// </summary>
public class AnyAggregationCalculationsData : AggregationCalculationsData<bool>
{
    /// <summary>
    /// Gets or sets the boolean result of the aggregation calculation.
    /// Represents whether any condition evaluated in the aggregation process
    /// satisfies the defined predicate.
    /// </summary>
    public bool Result { get; set; } 

    /// <inheritdoc />
    public override bool GetResult()
    {
        return Result;
    }
}