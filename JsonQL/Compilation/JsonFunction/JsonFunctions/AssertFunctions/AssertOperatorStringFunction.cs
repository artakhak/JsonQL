// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

/// <summary>
/// Represents a string operator assertion function that validates the satisfied condition
/// for a given JSON data model. This class ensures that the evaluated string value of an operator
/// adheres to specific assertion logic defined by the implemented functionality.
/// </summary>
/// <remarks>
/// Inherits from <see cref="StringJsonFunctionAbstr"/> to leverage base implementations
/// for JSON functions handling string operations. It overrides necessary methods to provide
/// custom parsing and validation for the asserted string operator function.
/// </remarks>
public class AssertOperatorStringFunction : StringJsonFunctionAbstr
{
    private readonly IStringJsonFunction _assertedOperatorFunction;

    /// <summary>
    /// Represents a specific implementation of <see cref="StringJsonFunctionAbstr"/> designed to validate and assert
    /// the correctness of string-based operator functions in a JSON function execution context.
    /// </summary>
    /// <param name="functionName">The name of the function being asserted.</param>
    /// <param name="assertedOperatorFunction">The operator function that this implementation will assert for validity.</param>
    /// <param name="jsonFunctionContext">The evaluation context in which the JSON function is operating.</param>
    /// <param name="lineInfo">Optional line information for error reporting and debugging in the JSON context.</param>
    public AssertOperatorStringFunction(string functionName, IStringJsonFunction assertedOperatorFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _assertedOperatorFunction = assertedOperatorFunction;
    }

    /// <inheritdoc />
    public override IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return AssertOperatorFunctionHelpers.GetParseResultWithErrorIfValueIsNull(
            _assertedOperatorFunction.EvaluateStringValue(rootParsedValue, compiledParentRootParsedValues, contextData), LineInfo);
    }
}