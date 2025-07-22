using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// NOTE: We can extend <see cref="DoubleJsonFunctionAbstr"/> to make things easier, but we want to demonstrate that tye implementation
/// does not have to extend <see cref="DoubleJsonFunctionAbstr"/> (adn even <see cref="JsonFunctionAbstr"/>).
/// The only requirement is to implement <see cref="IJsonFunction"/>.
/// </summary>
public class IncrementByTwoPrefixOperatorFunction : JsonFunctionAbstr, IDoubleJsonFunction
{
    private readonly IJsonFunction _operand1;

    public IncrementByTwoPrefixOperatorFunction(string functionName, IJsonFunction operand1, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _operand1 = operand1;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateDoubleValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <inheritdoc />
    public IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _operand1.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<double?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Double, out var comparableValue) ||
            comparableValue.Value is not double doubleValue)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>(doubleValue + 2);
    }
}
