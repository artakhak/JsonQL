using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;

public class IsEvenPostfixOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
   
    public IsEvenPostfixOperatorFunction(string operatorName, IJsonFunction jsonFunction, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedOperand1ValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedOperand1ValueResult.Errors.Count > 0)
            return new ParseResult<bool?>(evaluatedOperand1ValueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedOperand1ValueResult.Value, null, out var jsonComparable))
        {
            return new ParseResult<bool?>((bool?)null);
        }

        if (jsonComparable.Value is not double doubleValue)
            return new ParseResult<bool?>((bool?)null);

        var intValue = (int) doubleValue;

        if (Math.Abs(doubleValue - intValue) > 0.000001)
            return new ParseResult<bool?>(false);

        return new ParseResult<bool?>(intValue % 2 == 0);
    }
}