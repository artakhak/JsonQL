// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function used to evaluate whether a JSON value is not null or undefined.
/// </summary>
/// <remarks>
/// This class extends <see cref="BooleanJsonFunctionAbstr"/> and provides a specific implementation
/// for checking if a JSON value is not null. It leverages the internal functionality
/// of <see cref="IsNullUndefinedFunctionHelpers"/> to perform the operation.
/// </remarks>
public class IsNotNullOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;

    /// <summary>
    /// Represents a JSON function that evaluates whether a specified JSON value is not null.
    /// </summary>
    /// <param name="operatorName">The operator's name.</param>
    /// <param name="jsonValuePathJsonFunction">The JSON function that extracts the value path to evaluate.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function value.</param>
    /// <param name="lineInfo">Optional line information about where the function is declared.</param>
    public IsNotNullOperatorFunction(string operatorName, IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonValuePathJsonFunction = jsonValuePathJsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = IsNullUndefinedFunctionHelpers.IsNull(rootParsedValue, compiledParentRootParsedValues, contextData,
            _jsonValuePathJsonFunction);

        if (valueResult.Errors.Count > 0)
            return valueResult;

        if (valueResult.Value == null)
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("The value failed to parse.", this.LineInfo)));

        return new ParseResult<bool?>(!valueResult.Value.Value);
    }
}