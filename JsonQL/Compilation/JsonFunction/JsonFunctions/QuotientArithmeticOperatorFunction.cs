using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class QuotientArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    public QuotientArithmeticOperatorFunction(IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(JsonOperatorNames.AddOperator, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<object?> Calculate(double operand1Value, double operand2Value)
    {
        return new ParseResult<object?>(operand1Value % operand2Value);
    }
}