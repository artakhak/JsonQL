using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <inheritdoc />
public class AbsoluteValueJsonFunction : DoubleJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    public AbsoluteValueJsonFunction(string functionName, IJsonFunction jsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    protected override IParseResult<double?> GetDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<double?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Double, out var comparableValue) ||
            comparableValue.Value is not double doubleValue)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>(Math.Abs(doubleValue));
    }
}