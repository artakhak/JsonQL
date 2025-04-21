using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class TextLengthJsonFunction: JsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    public TextLengthJsonFunction(string functionName, IJsonFunction jsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var result = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result.Errors.Count > 0)
            return new ParseResult<object?>(result.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(result.Value, TypeCode.String, out var jsonComparable))
            return new ParseResult<string?>((string?)null);

        var length = jsonComparable.Value.ToString()?.Length;

        if (length == null)
            return new ParseResult<string?>((string?)null);

        return new ParseResult<object?>((double)length);
    }
}