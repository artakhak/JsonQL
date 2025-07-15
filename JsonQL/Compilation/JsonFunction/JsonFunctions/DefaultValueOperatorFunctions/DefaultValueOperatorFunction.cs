// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

/// <summary>
/// Represents a JSON function that evaluates a main value and a default value, and returns the main value if it is valid or the default value otherwise.
/// </summary>
/// <remarks>
/// This class is part of the default value operator functions in the JsonQL framework and provides a mechanism for handling fallback values during JSON parsing and evaluation.
/// </remarks>
public class DefaultValueOperatorFunction : JsonFunctionAbstr
{
    private readonly IJsonFunction _mainValueJsonFunction;
    private readonly IJsonFunction _defaultValueJsonFunction;

    /// <summary>
    /// Represents a base class for functions that can provide a default value if the main value is not available.
    /// </summary>
    /// <param name="operatorName">The operator name associated with the function.</param>
    /// <param name="mainValueJsonFunction">The primary JSON function to evaluate the main value.</param>
    /// <param name="defaultValueJsonFunction">The JSON function to provide a default value if the main value is unavailable.</param>
    /// <param name="jsonFunctionContext">The context used for evaluating JSON function values.</param>
    /// <param name="lineInfo">Optional information about the line and position in the source for error reporting.</param>
    public DefaultValueOperatorFunction(string operatorName, IJsonFunction mainValueJsonFunction, IJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        _mainValueJsonFunction = mainValueJsonFunction;
        _defaultValueJsonFunction = defaultValueJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _mainValueJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return valueResult;

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, _mainValueJsonFunction.TryGetTypeCode(), out var comparable))
            return new ParseResult<object?>(comparable.Value);

        valueResult = _defaultValueJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return valueResult;

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, _defaultValueJsonFunction.TryGetTypeCode(), out comparable))
            return new ParseResult<object?>(comparable.Value);

        return valueResult;
    }
}