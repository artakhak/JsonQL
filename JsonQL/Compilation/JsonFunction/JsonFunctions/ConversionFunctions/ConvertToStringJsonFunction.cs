using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

/// <summary>
/// Represents a function that converts an input value into a string representation in JSON format.
/// </summary>
/// <remarks>
/// This class extends the <see cref="StringJsonFunctionAbstr"/> to implement custom logic for converting
/// various types of JSON-compatible input values into their respective string representations.
/// If the conversion process encounters errors, it can optionally assert them through the
/// provided assertion function.
/// </remarks>
public class ConvertToStringJsonFunction : StringJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    /// <summary>
    /// Represents a JSON function to handle conversion of values to string type.
    /// </summary>
    /// <param name="functionName">The name of the function being implemented.</param>
    /// <param name="jsonFunction">The JSON function instance providing the value to convert to a string.</param>
    /// <param name="assertIfConversionFailsJsonFunction">An optional Boolean JSON function to assert if conversion to string fails.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function value.</param>
    /// <param name="lineInfo">Optional information about the source line for debugging and error tracking.</param>
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