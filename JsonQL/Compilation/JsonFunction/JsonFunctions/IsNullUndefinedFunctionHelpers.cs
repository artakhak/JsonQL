// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Provides helper methods to evaluate whether a JSON value is null or undefined during
/// the execution of JSON functions.
/// </summary>
public static class IsNullUndefinedFunctionHelpers
{
    /// <summary>
    /// Determines whether the specified value or path result is undefined.
    /// </summary>
    /// <param name="rootParsedValue">
    /// The root parsed value to evaluate.
    /// </param>
    /// <param name="compiledParentRootParsedValues">
    /// The list of compiled parent root parsed values.
    /// </param>
    /// <param name="contextData">
    /// The evaluation context data, if available.
    /// </param>
    /// <param name="jsonFunction">
    /// The JSON function used to evaluate the value or path result.
    /// </param>
    /// <returns>
    /// A parse result containing a nullable boolean indicating whether the value or path is undefined.
    /// </returns>
    public static IParseResult<bool?> IsUndefined(IRootParsedValue rootParsedValue, 
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData,
        IJsonFunction jsonFunction)
    {
        var valueResult = jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<bool?>(valueResult.Errors);

        if (valueResult.Value is IJsonValuePathLookupResult jsonValuePathLookupResult)
        {
            if (jsonValuePathLookupResult is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult &&
                !singleItemJsonValuePathLookupResult.IsValidPath)
                return new ParseResult<bool?>(true);

            return new ParseResult<bool?>(false);
        }

        return new ParseResult<bool?>(valueResult.Value == null);
    }

    /// <summary>
    /// Determines whether the specified value or path result is null.
    /// </summary>
    /// <param name="rootParsedValue">
    /// The root parsed value to evaluate.
    /// </param>
    /// <param name="compiledParentRootParsedValues">
    /// The list of compiled parent root parsed values.
    /// </param>
    /// <param name="contextData">
    /// The evaluation context data, if available.
    /// </param>
    /// <param name="jsonValuePathJsonFunction">
    /// The JSON function used to evaluate the value or path result.
    /// </param>
    /// <returns>
    /// A parse result containing a nullable boolean indicating whether the value or path is null.
    /// </returns>
    public static IParseResult<bool?> IsNull(IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction)
    {
        var pathResult = jsonValuePathJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (pathResult.Errors.Count > 0)
            return new ParseResult<bool?>(pathResult.Errors);

        return new ParseResult<bool?>(pathResult.Value is ISingleItemJsonValuePathLookupResult 
            {ParsedValue: IParsedSimpleValue {IsString: false, Value: null}}
        );
    }
}