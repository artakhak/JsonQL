// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

/// <summary>
/// Represents an implementation of a boolean JSON function that asserts an operator's behavior.
/// </summary>
public class AssertOperatorBooleanFunction : BooleanJsonFunctionAbstr
{
    private readonly IBooleanJsonFunction _assertedOperatorFunction;

    /// <summary>
    /// Represents a specialized boolean JSON function designed to validate or assert
    /// the operations performed by another boolean JSON function provided as a dependency.
    /// </summary>
    /// <param name="functionName">The name assigned to the assertion function.</param>
    /// <param name="assertedOperatorFunction">The boolean JSON function whose operations are evaluated and asserted.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function's value.</param>
    /// <param name="lineInfo">Optional line information for debugging and tracing the function's location in the source.</param>
    public AssertOperatorBooleanFunction(string functionName, IBooleanJsonFunction assertedOperatorFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _assertedOperatorFunction = assertedOperatorFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return AssertOperatorFunctionHelpers.GetParseResultWithErrorIfValueIsNull(
            _assertedOperatorFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData), LineInfo);
    }
}