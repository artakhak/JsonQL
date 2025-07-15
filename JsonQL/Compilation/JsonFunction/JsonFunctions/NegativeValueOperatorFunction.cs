// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function that computes the negation of a numeric value.
/// This class implements behavior for applying a negative unary operator
/// to a numeric value, specifically for JSON function evaluation contexts.
/// It is designed to work with double-precision floating-point values.
/// </summary>
public class NegativeValueOperatorFunction : DoubleJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents a function that applies a negative value operation within a JSON evaluation context.
    /// </summary>
    /// <param name="operatorName">The name of the operator applied in the function.</param>
    /// <param name="jsonFunction">The JSON function implementation used for value operation.</param>
    /// <param name="jsonFunctionContext">The context used for evaluating the JSON function values.</param>
    /// <param name="lineInfo">Optional line information for debugging or error reporting.</param>
    /// <remarks>
    /// This function encapsulates the behavior of performing negation operations on JSON values, extending the double-based functionality
    /// </remarks>
    /// provided by the base class. It makes use of the operator name, function context, and optional line details to facilitate computation.
    public NegativeValueOperatorFunction(string operatorName, IJsonFunction jsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<double?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Double, out var comparableValue) ||
            comparableValue.Value is not double doubleValue)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>(-doubleValue);
    }
}