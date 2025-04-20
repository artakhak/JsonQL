using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.JsonFunction.JsonFunctions.ConversionFunctions;

public class ConvertToStringJsonFunction : StringJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    public ConvertToStringJsonFunction(string functionName, IJsonFunction jsonFunction,
        IBooleanJsonFunction? assertIfConversionFailsJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
        _assertIfConversionFailsJsonFunction = assertIfConversionFailsJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<string?> GetStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedValueResult.Errors.Count > 0)
            return new ParseResult<string?>(evaluatedValueResult.Errors);

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedValueResult.Value, null, out var jsonComparable))
        {
            if (jsonComparable.Value is string stringValue)
                return new ParseResult<string?>(stringValue);

            if (jsonComparable.Value is DateTime dateTime)
            {
                return new ParseResult<string?>((string?)DateTimeOperationsAmbientContext.Context.ToString(dateTime));
            }
            
            if (jsonComparable.Value is bool)
            {
                return new ParseResult<string?>(jsonComparable.Value.ToString()!.ToLower());
            }

            return new ParseResult<string?>(jsonComparable.Value.ToString());
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<string?>((string?)null);

        return ConversionJsonFunctionHelpers.GetParseResultForConversionError<string?>(TypeCode.String.ToString(),
                   _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.Evaluate(rootParsedValue,
                       compiledParentRootParsedValues, contextData), this.LineInfo) ??
               new ParseResult<string?>((string?)null);
    }
}