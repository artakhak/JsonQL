using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function that performs the quotient (modulus) operation between two numeric operands.
/// </summary>
/// <remarks>
/// This class evaluates the modulus of two numeric operands using the '%' operator.
/// It inherits from <c>BinaryNumericArithmeticOperationOperatorFunctionAbstr</c>.
/// </remarks>
public class QuotientArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    /// <summary>
    /// Represents an arithmetic division operation designed to take two numeric operands
    /// within a JSON function context and execute a division operation between them.
    /// </summary>
    /// <param name="operand1">The first numeric input operand to the division operation.</param>
    /// <param name="operand2">The second numeric input operand, which serves as the divisor in the operation.</param>
    /// <param name="jsonFunctionContext">The context used for evaluation of JSON function values.</param>
    /// <param name="lineInfo">Optional line information providing metadata about the location in the source.</param>
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