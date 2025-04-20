using JsonQL.JsonExpression;
using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.JsonFunction.JsonFunctions;

public class NegateBooleanValueOperator : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    public NegateBooleanValueOperator(string operatorName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<bool?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Boolean, out var comparableValue) ||
            comparableValue.Value is not bool boolValue)
            return new ParseResult<bool?>((bool?)null);

        return new ParseResult<bool?>(!boolValue);
    }
}