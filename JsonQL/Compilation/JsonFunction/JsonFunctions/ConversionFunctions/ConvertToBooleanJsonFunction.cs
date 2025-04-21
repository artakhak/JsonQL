using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

public class ConvertToBooleanJsonFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    public ConvertToBooleanJsonFunction(string functionName, IJsonFunction jsonFunction, IBooleanJsonFunction? assertIfConversionFailsJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
        _assertIfConversionFailsJsonFunction = assertIfConversionFailsJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedValueResult.Errors.Count > 0)
            return new ParseResult<bool?>(evaluatedValueResult.Errors);
       
        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedValueResult.Value, null, out var jsonComparable))
        {
            if (jsonComparable.Value is bool booleanValue)
                return new ParseResult<bool?>(booleanValue);

            if (jsonComparable.Value is string stringValue)
            {
                switch (stringValue)
                {
                    case "true":
                    case "True":
                        return new ParseResult<bool?>(true);

                    case "false":
                    case "False":
                        return new ParseResult<bool?>(false);
                 
                }
            }
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<bool?>((bool?)null);

        return 
            ConversionJsonFunctionHelpers.GetParseResultForConversionError<bool?>(TypeCode.Boolean.ToString(),
            _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData),
            this.LineInfo) ?? new ParseResult<bool?>((bool?)null);
    }
}