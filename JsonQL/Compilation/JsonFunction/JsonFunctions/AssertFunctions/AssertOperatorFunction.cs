// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

/// <summary>
/// Represents a specific implementation of a JSON function, used to assert and ensure the validity of an evaluated
/// value within a JSON context. The class uses a delegated `IJsonFunction` to evaluate the asserted value and provides
/// functionality for error handling if the asserted value is null.
/// </summary>
public class AssertOperatorFunction : JsonFunctionAbstr
{
    private readonly IJsonFunction _assertedValueJsonFunction;

    /// <summary>
    /// Represents an assert operation function that validates a value by interacting with the asserted JSON function.
    /// Inherits from <c>JsonFunctionAbstr</c>. Implements specific validation logic in its overridden members.
    /// </summary>
    /// <param name="functionName">The name of the function being asserted.</param>
    /// <param name="assertedValueJsonFunction">The JSON function to assert the value against during evaluation.</param>
    /// <param name="jsonFunctionContext">The evaluation context associated with JSON functions.</param>
    /// <param name="lineInfo">Optional line information for error reporting and debugging.</param>
    public AssertOperatorFunction(string functionName, IJsonFunction assertedValueJsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _assertedValueJsonFunction = assertedValueJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return AssertOperatorFunctionHelpers.GetParseResultWithErrorIfValueIsNull(
            _assertedValueJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData), LineInfo);
    }
}