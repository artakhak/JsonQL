using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;

public class ReverseTextAndAddMarkersJsonFunction : StringJsonFunctionAbstr
{
    private readonly IJsonFunction _stringJsonFunction;
    private readonly IBooleanJsonFunction? _addMarkersJsonFunction;

    public ReverseTextAndAddMarkersJsonFunction(string functionName,
        IJsonFunction stringJsonFunction,
        IBooleanJsonFunction? addMarkersJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _stringJsonFunction = stringJsonFunction;
        _addMarkersJsonFunction = addMarkersJsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var reversedValue = _stringJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (reversedValue.Errors.Count > 0)
            return new ParseResult<string?>(reversedValue.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(reversedValue.Value, null, out var jsonComparable))
            return new ParseResult<string?>((string?)null);

        if (jsonComparable.Value is not string evaluatedStringValue)
        {
            return new ParseResult<string?>([new JsonObjectParseError("Expected a string value", LineInfo)]);
        }

        var textCharacters = evaluatedStringValue.ToCharArray();
        Array.Reverse(textCharacters);

        var reversedText = new string(textCharacters);

        if (_addMarkersJsonFunction != null)
        {
            var addMarkersResult = _addMarkersJsonFunction.EvaluateBooleanValue(rootParsedValue, compiledParentRootParsedValues, contextData);

            if (addMarkersResult.Errors.Count > 0)
                return new ParseResult<string?>(addMarkersResult.Errors);

            if (!(addMarkersResult.Value ?? true))
                return new ParseResult<string?>(reversedText);
        }

        return new ParseResult<string?>(string.Concat("#", reversedText, "#"));
    }
}