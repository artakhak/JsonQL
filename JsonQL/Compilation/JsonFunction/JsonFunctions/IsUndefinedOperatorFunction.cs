// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function operator that determines if a given value is undefined.
/// This class extends the <see cref="BooleanJsonFunctionAbstr"/> abstract class and implements
/// logic to evaluate the "undefined" state of a value in a JSON context.
/// </summary>
public class IsUndefinedOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents a function that evaluates whether a JSON function or value is undefined.
    /// </summary>
    /// <remarks>
    /// Extends the <see cref="BooleanJsonFunctionAbstr"/> to provide a boolean result for undefined checks,
    /// using an inner JSON function and evaluation context.
    /// </remarks>
    /// <param name="operatorName">The name of the operator being evaluated.</param>
    /// <param name="jsonFunction">The JSON function to evaluate for undefined state.</param>
    /// <param name="jsonFunctionContext">The evaluation context for the JSON function.</param>
    /// <param name="lineInfo">Optional line information used for error reporting or debugging.</param>
    public IsUndefinedOperatorFunction(string operatorName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return IsNullUndefinedFunctionHelpers.IsUndefined(rootParsedValue, compiledParentRootParsedValues, contextData, _jsonFunction);
    }
}
