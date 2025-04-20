using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.JsonFunction.JsonFunctions.ConversionFunctions;

public class ConvertToDateJsonFunction : DateTimeJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    public ConvertToDateJsonFunction(string functionName, IJsonFunction jsonFunction,
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
            DateTime? parsedDate;
            if (jsonComparable.Value is DateTime date)
            {
                parsedDate = date;
            }
            else if (jsonComparable.Value is not string stringValue || !DateTimeOperationsAmbientContext.Context.TryParse(stringValue, out parsedDate))
            {
                parsedDate = null;
            }

            if (parsedDate != null)
                return new ParseResult<DateTime?>(DateTimeOperationsAmbientContext.Context.ConvertToDate(parsedDate.Value));
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<DateTime?>((DateTime?)null);

        return ConversionJsonFunctionHelpers.GetParseResultForConversionError<DateTime?> (TypeCode.DateTime.ToString(),
                _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.Evaluate(rootParsedValue, 
                    compiledParentRootParsedValues, contextData), this.LineInfo) ?? 
               new ParseResult<DateTime?>((DateTime?)null);
    }
}