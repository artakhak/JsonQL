using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

public class ConvertToDateTimeJsonFunction : DateTimeJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    public ConvertToDateTimeJsonFunction(string functionName, IJsonFunction jsonFunction,
        IBooleanJsonFunction? assertIfConversionFailsJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
        _assertIfConversionFailsJsonFunction = assertIfConversionFailsJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<DateTime?> GetDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedValueResult.Errors.Count > 0)
            return new ParseResult<DateTime?>(evaluatedValueResult.Errors);

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedValueResult.Value, null, out var jsonComparable))
        {
            if (jsonComparable.Value is DateTime dateTime)
            {
                return new ParseResult<DateTime?>(dateTime);
            }

            if (jsonComparable.Value is string stringValue && DateTimeOperationsAmbientContext.Context.TryParse(stringValue, out var parsedDateTime))
            {
                return new ParseResult<DateTime?>(parsedDateTime);
            }
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<DateTime?>((DateTime?)null);

        return ConversionJsonFunctionHelpers.GetParseResultForConversionError<DateTime?>(TypeCode.DateTime.ToString(),
                   _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.Evaluate(rootParsedValue,
                       compiledParentRootParsedValues, contextData), this.LineInfo) ??
               new ParseResult<DateTime?>((DateTime?)null);
    }
}