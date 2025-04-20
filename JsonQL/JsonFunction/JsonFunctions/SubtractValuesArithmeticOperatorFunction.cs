using JsonQL.JsonExpression;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class SubtractValuesArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    public SubtractValuesArithmeticOperatorFunction(IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : 
        base(JsonOperatorNames.SubtractOperator, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<object?> Calculate(double operand1Value, double operand2Value)
    {
        return new ParseResult<object?>(operand1Value - operand2Value);
    }
}