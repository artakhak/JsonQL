// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

/// <summary>
/// Provides helper methods for assertion operations within JSON function evaluations.
/// This class includes functionality to validate parse results and handle errors when
/// required conditions, such as non-null values, are not met.
/// </summary>
public static class AssertOperatorFunctionHelpers
{
    /// <summary>
    /// Returns the given <paramref name="parseResult"/> if it contains errors or its value is not null,
    /// otherwise returns a new parse result with an error indicating that the value must not be null.
    /// </summary>
    /// <param name="parseResult">The parse result to evaluate.</param>
    /// <param name="lineInfo">The source line information for constructing error details, if applicable.</param>
    /// <typeparam name="T">The type of the parse result value.</typeparam>
    /// <returns>
    /// The original parse result if it contains errors or a non-null value, or a new parse result
    /// with an associated error if the value is null.
    /// </returns>
    public static IParseResult<T?> GetParseResultWithErrorIfValueIsNull<T>(IParseResult<T?> parseResult, IJsonLineInfo? lineInfo)
    {
        if (parseResult.Errors.Count > 0)
            return parseResult;

        bool isValueNull = parseResult.Value == null;

        if (!isValueNull)
        {
            var jsonValuePathLookupResult = 
                (parseResult as IJsonValuePathLookupResult)??(parseResult.Value as IJsonValuePathLookupResult);

            if (jsonValuePathLookupResult is {HasValue: false})
                isValueNull = true;
        }

        if (isValueNull)
            return new ParseResult<T>(CollectionExpressionHelpers.Create(new JsonObjectParseError("Value not-null assertion failed", lineInfo)));

        return parseResult;
    }
}