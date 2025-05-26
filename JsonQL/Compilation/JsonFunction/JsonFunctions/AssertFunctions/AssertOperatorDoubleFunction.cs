// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

/// <summary>
/// Represents a JSON function that asserts the result of another operator function, specifically when expecting a double value.
/// </summary>
/// <remarks>
/// This class extends <see cref="DoubleJsonFunctionAbstr"/> and builds upon an asserted operator function that returns
/// double values.
/// </remarks>
public class AssertOperatorDoubleFunction : DoubleJsonFunctionAbstr
{
    private readonly IDoubleJsonFunction _assertedOperatorFunction;

    /// <summary>
    /// Represents a double JSON function that asserts the behavior of a specific operator function.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="assertedOperatorFunction">The operator function being asserted.</param>
    /// <param name="jsonFunctionContext">The evaluation context for the JSON function.</param>
    /// <param name="lineInfo">Optional line information for error reporting.</param>
    public AssertOperatorDoubleFunction(string functionName, IDoubleJsonFunction assertedOperatorFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _assertedOperatorFunction = assertedOperatorFunction;
    }

    /// <inheritdoc />
    public override IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return AssertOperatorFunctionHelpers.GetParseResultWithErrorIfValueIsNull(
            _assertedOperatorFunction.EvaluateDoubleValue(rootParsedValue, compiledParentRootParsedValues, contextData), LineInfo);
    }
}
