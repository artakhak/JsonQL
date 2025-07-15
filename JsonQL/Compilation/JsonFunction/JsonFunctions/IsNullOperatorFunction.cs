// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function to evaluate whether a given JSON value is null. This class is derived
/// from <see cref="BooleanJsonFunctionAbstr"/>, inheriting the functionality to handle boolean-based
/// JSON function evaluations. It is used to validate whether the specified JSON value or path evaluates to null.
/// </summary>
public class IsNullOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;

    /// <summary>
    /// Represents a function that evaluates whether a given JSON value is null.
    /// </summary>
    /// <remarks>
    /// This class is a specific implementation of <see cref="BooleanJsonFunctionAbstr"/>
    /// and operates as a JSON function for evaluating null conditions on values
    /// based on the provided JSON path and context.
    /// </remarks>
    /// <param name="operatorName">
    /// The name of the operator represented by the function.
    /// </param>
    /// <param name="jsonValuePathJsonFunction">
    /// An instance of <see cref="IJsonValuePathJsonFunction"/> that represents
    /// the JSON path being evaluated.
    /// </param>
    /// <param name="jsonFunctionContext">
    /// The context used during the evaluation of the JSON function, provided as an
    /// implementation of <see cref="IJsonFunctionValueEvaluationContext"/>.
    /// </param>
    /// <param name="lineInfo">
    /// Optional parameter that provides line information for error tracing or logging,
    /// implemented by <see cref="IJsonLineInfo"/>.
    /// </param>
    public IsNullOperatorFunction(string operatorName, IJsonValuePathJsonFunction jsonValuePathJsonFunction, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonValuePathJsonFunction = jsonValuePathJsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return IsNullUndefinedFunctionHelpers.IsNull(rootParsedValue, compiledParentRootParsedValues, contextData,
            _jsonValuePathJsonFunction);
    }
}