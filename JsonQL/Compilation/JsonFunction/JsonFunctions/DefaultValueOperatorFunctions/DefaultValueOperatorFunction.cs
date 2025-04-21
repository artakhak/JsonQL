using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

public class DefaultValueOperatorFunction : JsonFunctionAbstr
{
    private readonly IJsonFunction _mainValueJsonFunction;
    private readonly IJsonFunction _defaultValueJsonFunction;

    public DefaultValueOperatorFunction(string operatorName, IJsonFunction mainValueJsonFunction, IJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        _mainValueJsonFunction = mainValueJsonFunction;
        _defaultValueJsonFunction = defaultValueJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _mainValueJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return valueResult;

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, _mainValueJsonFunction.TryGetTypeCode(), out var comparable))
            return new ParseResult<object?>(comparable.Value);

        valueResult = _defaultValueJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return valueResult;

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, _defaultValueJsonFunction.TryGetTypeCode(), out comparable))
            return new ParseResult<object?>(comparable.Value);

        return valueResult;
    }
}