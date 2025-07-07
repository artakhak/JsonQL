// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that evaluates whether a given numeric value is an odd number.
/// This function checks the numeric value provided during execution to determine
/// if it is both an integer and an odd number.
/// </summary>
/// <remarks>
/// The function internally uses a comparison and modulo operation to verify the odd property of the value.
/// If the value cannot be converted to a valid numeric format, it will fail the evaluation and return null.
/// </remarks>
public class IsOddValueJsonFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents a JSON function that evaluates whether a value is odd.
    /// Inherits from <see cref="BooleanJsonFunctionAbstr"/>.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="jsonFunction">The JSON function implementation.</param>
    /// <param name="jsonFunctionContext">The context for JSON function value evaluation.</param>
    /// <param name="lineInfo">Optional line information for JSON function parsing.</param>
    public IsOddValueJsonFunction(string functionName, IJsonFunction jsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<bool?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Double, out var comparableValue) ||
            comparableValue.Value is not double doubleValue)
            return new ParseResult<bool?>((bool?)null);

        var intValue = (int) doubleValue;
        return new ParseResult<bool?>(doubleValue <= intValue && Math.Abs(intValue) % 2 == 1);
    }
}