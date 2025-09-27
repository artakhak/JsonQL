// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.Query;

/// <summary>
/// Represents the result of an object query operation, including the queried value and any associated errors or warnings.
/// </summary>
public interface IObjectQueryResult
{
    /// <summary>
    /// Gets the collection of errors and warnings associated with the query result.
    /// This property provides details regarding compilation errors, execution errors, and warnings
    /// encountered during the query processing.
    /// </summary>
    IQueryResultErrorsAndWarnings ErrorsAndWarnings { get; }
}

/// <summary>
/// Represents the result of an object query operation, including the queried value and any associated errors or warnings.
/// </summary>
public interface IObjectQueryResult<TQueryObject>: IObjectQueryResult
{
    /// <summary>
    /// Gets the value of the query result. If the query does not resolve to a single object, this property will return null.
    /// </summary>
    TQueryObject? Value { get; }
}