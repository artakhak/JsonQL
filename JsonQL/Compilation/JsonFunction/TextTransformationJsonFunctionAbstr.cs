using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

public abstract class TextTransformationJsonFunctionAbstr : StringJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    protected TextTransformationJsonFunctionAbstr(string functionName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<string?> GetStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluateResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluateResult.Errors.Count > 0)
            return new ParseResult<string?>(evaluateResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluateResult.Value, null, out var jsonComparable))
            return new ParseResult<string?>((string?)null);

        return ConvertString(jsonComparable.Value.ToString() ?? String.Empty);
    }

    protected abstract IParseResult<string> ConvertString(string value);
}