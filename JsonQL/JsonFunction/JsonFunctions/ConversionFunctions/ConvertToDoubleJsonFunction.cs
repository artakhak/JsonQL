using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.JsonFunction.JsonFunctions.ConversionFunctions;

public class ConvertToDoubleJsonFunction : DoubleJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    public ConvertToDoubleJsonFunction(string functionName, IJsonFunction jsonFunction,
        IBooleanJsonFunction? assertIfConversionFailsJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
        _assertIfConversionFailsJsonFunction = assertIfConversionFailsJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<double?> GetDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedValueResult.Errors.Count > 0)
            return new ParseResult<double?>(evaluatedValueResult.Errors);

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedValueResult.Value, null, out var jsonComparable))
        {
            if (jsonComparable.Value is double doubleValue)
            {
                return new ParseResult<double?>(doubleValue);
            }

            if (jsonComparable.Value is string stringValue &&
                double.TryParse(stringValue, out doubleValue))
            {
                return new ParseResult<double?>(doubleValue);
            }
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<double?>((double?)null);

        return ConversionJsonFunctionHelpers.GetParseResultForConversionError<double?>(TypeCode.Double.ToString(),
                   _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.Evaluate(rootParsedValue,
                       compiledParentRootParsedValues, contextData), this.LineInfo) ??
               new ParseResult<double?>((double?)null);
    }
}