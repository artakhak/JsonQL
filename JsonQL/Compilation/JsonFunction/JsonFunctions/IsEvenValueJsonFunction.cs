// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that evaluates whether a given numeric value is even.
/// This function processes a provided numeric input, evaluates its parity, and
/// determines if it is an even number.
/// </summary>
public class IsEvenValueJsonFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _evaluatedValue;

    /// <summary>
    /// Represents a JSON function that evaluates whether a given value is even.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="evaluatedValue">The JSON function representing the value to be evaluated.</param>
    /// <param name="jsonFunctionContext">The context used during the evaluation of the JSON function.</param>
    /// <param name="lineInfo">Optional line information to assist with debugging and error reporting.</param>
    public IsEvenValueJsonFunction(string functionName, IJsonFunction evaluatedValue, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _evaluatedValue = evaluatedValue;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _evaluatedValue.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<bool?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Double, out var comparableValue) ||
            comparableValue.Value is not double doubleValue)
            return new ParseResult<bool?>((bool?)null);

        var intValue = (int)doubleValue;
        return new ParseResult<bool?>(doubleValue <= intValue && Math.Abs(intValue) % 2 == 0);
    }
}