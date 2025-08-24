// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents an abstract base class for evaluating JSON value path lookup results from a simple value.
/// </summary>
/// <typeparam name="T">The type of the value being evaluated.</typeparam>
public abstract class EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr<T> : JsonFunctionAbstr, IEvaluatesJsonValuePathLookupResult
{
    private readonly IStringFormatter _stringFormatter;

    protected EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr(IStringFormatter stringFormatter, string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _stringFormatter = stringFormatter;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);
    }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var parseResult = this.EvaluateParseResult(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (parseResult.Errors.Count > 0)
            return new ParseResult<IJsonValuePathLookupResult>(parseResult.Errors);

        if (parseResult.Value == null)
            return new ParseResult<IJsonValuePathLookupResult>(CollectionExpressionHelpers.Create(new JsonObjectParseError($"Failed to parse [{typeof(T).FullName}] value", this.LineInfo)));

        if (!this._stringFormatter.TryFormat(parseResult.Value, out var formattedValue))
        {
            return new ParseResult<IJsonValuePathLookupResult>(CollectionExpressionHelpers.Create(new JsonObjectParseError($"Failed to format a [{typeof(T).FullName}] value", this.LineInfo)));
        }

        return new ParseResult<IJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForValidPath(
            new ParsedCalculatedSimpleValue(rootParsedValue, formattedValue, IsStringJsonValue)));
    }

    /// <summary>
    /// Evaluates the parse result using the provided root parsed value, parent root parsed values, and optional evaluation context data.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value to evaluate.</param>
    /// <param name="compiledParentRootParsedValues">The collection of parent root parsed values used during evaluation.</param>
    /// <param name="contextData">Optional evaluation context data providing additional information for the operation.</param>
    /// <returns>An implementation of <see cref="IParseResult{T}"/> containing the evaluation result.</returns>
    protected abstract IParseResult<T> EvaluateParseResult(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);

    /// Indicates whether the evaluated value is expected to be treated as a string
    /// JSON value within the context of JSON path lookup result evaluation.
    /// This property is utilized within implementations of
    /// `EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr<T>`
    /// to determine how the parsed and formatted value should be processed.
    /// If set to `true`, the value is treated as a string JSON value.
    /// If set to `false`, it is treated differently based on the derived implementation.
    /// Typical derived implementations might override this property to specify
    /// behavior for specific value types such as strings, dates, numbers, or booleans.
    protected abstract bool IsStringJsonValue { get; }
}