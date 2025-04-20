using JsonQL.JsonExpression;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class MultiplyValuesArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    public MultiplyValuesArithmeticOperatorFunction(IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(JsonOperatorNames.MultiplyOperator, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<object?> Calculate(double operand1Value, double operand2Value)
    {
        return new ParseResult<object?>(operand1Value * operand2Value);
    }
}