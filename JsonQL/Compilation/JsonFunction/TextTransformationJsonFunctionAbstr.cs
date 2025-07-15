// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// An abstract base class that represents a JSON function focused on text transformation.
/// This class inherits from <see cref="StringJsonFunctionAbstr"/> and provides a mechanism
/// to transform string values using its template method pattern.
/// </summary>
public abstract class TextTransformationJsonFunctionAbstr : StringJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents an abstract base class for JSON text transformation functions.
    /// This class provides the structural and functional foundation for specific text transformation
    /// operations within JSON, and operates using the template method pattern.
    /// </summary>
    /// <remarks>
    /// Derived classes are responsible for defining the specific text transformation logic by implementing
    /// the <see cref="ConvertString"/> abstract method. This class also overrides the string value
    /// retrieval mechanism provided by its base class.
    /// </remarks>
    protected TextTransformationJsonFunctionAbstr(string functionName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluateResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluateResult.Errors.Count > 0)
            return new ParseResult<string?>(evaluateResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluateResult.Value, null, out var jsonComparable))
            return new ParseResult<string?>((string?)null);

        return ConvertString(jsonComparable.Value.ToString() ?? String.Empty);
    }

    /// <summary>
    /// Converts the input string value to a desired format or transformation based on the implemented logic.
    /// Derived classes must implement this method to specify the transformation behavior for the string.
    /// </summary>
    /// <param name="value">The input string value that needs to be transformed.</param>
    /// <returns>An <see cref="IParseResult{TValue}"/> containing the transformed string.</returns>
    protected abstract IParseResult<string> ConvertString(string value);
}