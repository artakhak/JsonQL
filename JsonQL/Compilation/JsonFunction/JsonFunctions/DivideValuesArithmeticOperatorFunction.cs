using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function that performs division operation on two numerical operands.
/// </summary>
/// <remarks>
/// This class is used to define and calculate the division of two numerical values.
/// It derives from the BinaryNumericArithmeticOperationOperatorFunctionAbstr base class,
/// inheriting its behavior and structure for binary numeric operations.
/// </remarks>
/// <seealso cref="BinaryNumericArithmeticOperationOperatorFunctionAbstr"/>
public class DivideValuesArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a function that performs division between two numeric operands in a JSON-based context.
    /// </summary>
    /// <param name="operand1">The first operand for the division operation.</param>
    /// <param name="operand2">The second operand for the division operation.</param>
    /// <param name="jsonFunctionContext">The evaluation context in which the function is executed.</param>
    /// <param name="lineInfo">Optional line information for debugging or error reporting.</param>
    public DivideValuesArithmeticOperatorFunction(IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(JsonOperatorNames.DivideOperator, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<object?> Calculate(double operand1Value, double operand2Value)
    {
        return new ParseResult<object?>(operand1Value / operand2Value);
    }
}