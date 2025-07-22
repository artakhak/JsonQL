// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

/// <summary>
/// Represents a function used to assert operations on DateTime values within the context of JSON functions.
/// </summary>
public class AssertOperatorDateTimeFunction : DateTimeJsonFunctionAbstr
{
    private readonly IDateTimeJsonFunction _assertedOperatorFunction;

    /// <summary>
    /// Represents an assertion operator function specific to DateTime values.
    /// </summary>
    /// <param name="functionName">The name of the function being asserted.</param>
    /// <param name="assertedOperatorFunction">The operator function to assert for the DateTime value.</param>
    /// <param name="jsonFunctionContext">The context in which the JSON function operates.</param>
    /// <param name="lineInfo">Optional line information for debugging or error reporting.</param>
    /// <remarks>
    /// This class inherits from <see cref="DateTimeJsonFunctionAbstr"/> and facilitates assertion logic for DateTime-based JSON functions.
    /// </remarks>
    public AssertOperatorDateTimeFunction(string functionName, IDateTimeJsonFunction assertedOperatorFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _assertedOperatorFunction = assertedOperatorFunction;
    }

    /// <inheritdoc />
    public override IParseResult<DateTime?> EvaluateDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return AssertOperatorFunctionHelpers.GetParseResultWithErrorIfValueIsNull(
            _assertedOperatorFunction.EvaluateDateTimeValue(rootParsedValue, compiledParentRootParsedValues, contextData), LineInfo);
    }
}
