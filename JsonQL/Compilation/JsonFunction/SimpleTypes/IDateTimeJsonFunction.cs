// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an interface for JSON functions that evaluate to a nullable <see cref="DateTime"/> result.
/// Implements the <see cref="IJsonFunction"/> base interface, providing specialized behavior for date and time operations.
/// </summary>
public interface IDateTimeJsonFunction : IJsonFunction
{
    /// <summary>
    /// Evaluates the function and returns a nullable <see cref="DateTime"/> result.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value associated with the JSON query.</param>
    /// <param name="compiledParentRootParsedValues">The compiled list of parent root parsed values relevant for the current evaluation.</param>
    /// <param name="contextData">The evaluation context data, providing additional information or configurations for the function evaluation. Can be null.</param>
    /// <returns>An implementation of <see cref="IParseResult{DateTime?}"/> containing the evaluated nullable <see cref="DateTime"/> value and any associated errors.</returns>
    IParseResult<DateTime?> EvaluateDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}