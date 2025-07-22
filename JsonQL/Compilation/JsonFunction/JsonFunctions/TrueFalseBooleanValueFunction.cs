// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a boolean value function that returns a predefined constant value.
/// </summary>
/// <remarks>
/// This class is used for JSON-based boolean value evaluation functions that always
/// return either true or false as their result. The constant boolean value is specified
/// during the creation of the function instance.
/// </remarks>
public class TrueFalseBooleanValueFunction: BooleanJsonFunctionAbstr
{
    private readonly bool _value;

    /// <summary>
    /// Represents a function that processes boolean JSON literals, always evaluating to a predefined true or false value.
    /// </summary>
    /// <param name="functionName">The name of the function as defined within the JSON context.</param>
    /// <param name="value">The boolean value that the function evaluates to.</param>
    /// <param name="jsonFunctionContext">The functional context in which this JSON function is executed.</param>
    /// <param name="lineInfo">Optional positional metadata related to the JSON source.</param>
    /// <remarks>
    /// This class is designed to encapsulate boolean literals in a JSON syntax tree, ensuring they consistently resolve
    /// to a constant true or false value during evaluation.
    /// </remarks>
    public TrueFalseBooleanValueFunction(string functionName, bool value, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        functionName, jsonFunctionContext, lineInfo)
    {
        _value = value;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<bool?>(_value);
    }
}
