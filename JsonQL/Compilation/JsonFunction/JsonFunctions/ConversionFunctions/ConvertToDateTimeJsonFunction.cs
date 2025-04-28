using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

/// <summary>
/// Represents a JSON function that converts an input value to a DateTime object.
/// </summary>
/// <remarks>
/// This class extends the <see cref="DateTimeJsonFunctionAbstr"/> and provides
/// functionality to evaluate and convert JSON input values into DateTime instances.
/// Includes error handling and optional conversion validation.
/// </remarks>
public class ConvertToDateTimeJsonFunction : DateTimeJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    /// <summary>
    /// Represents a JSON function that converts a value to a DateTime.
    /// </summary>
    /// <remarks>
    /// This class provides functionality to handle conversion of an input JSON value
    /// into a DateTime representation. It also supports optional assertion of conversion failure.
    /// </remarks>
    /// <param name="functionName">The name of the function being executed.</param>
    /// <param name="jsonFunction">The JSON function providing the input value for conversion.</param>
    /// <param name="assertIfConversionFailsJsonFunction">
    /// An optional JSON function to assert conditions if the conversion fails.
    /// </param>
    /// <param name="jsonFunctionContext">The context object for value evaluation during the function execution.</param>
    /// <param name="lineInfo">Optional information about the specific line in the JSON for error handling or debugging.</param>
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