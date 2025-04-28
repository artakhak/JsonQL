using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Abstract base class representing a binary arithmetic operation for numeric operands.
/// This class is designed to handle calculations involving two numeric values (doubles).
/// </summary>
/// <remarks>
/// Classes that inherit from this abstract class should implement the abstract <c>Calculate</c> method
/// to define specific arithmetic operations (e.g., addition, multiplication, etc.).
/// </remarks>
public abstract class BinaryNumericArithmeticOperationOperatorFunctionAbstr : BinaryArithmeticOperationOperatorFunctionAbstr
{
    protected BinaryNumericArithmeticOperationOperatorFunctionAbstr(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> Calculate(IJsonComparable operand1Value, IJsonComparable operand2Value)
    {
        if (operand1Value.Value is not double operand1DoubleValue || operand2Value.Value is not double operand2DoubleValue)
            return new ParseResult<object?>((bool?)null);

        try
        {
            return Calculate(operand1DoubleValue, operand2DoubleValue);
        }
        catch (Exception e) // OverflowException
        {
            var errorMessage = $"Failed to evaluate the operator [{this.FunctionName}] for values [{operand1Value}] and [{operand2Value}].";
            ThreadStaticLogging.Log.Error(errorMessage, e);
            return new ParseResult<object?>(CollectionExpressionHelpers.Create(new JsonObjectParseError(errorMessage, this.LineInfo)));
        }
    }

    /// <summary>
    /// Calculates the result of an arithmetic operation using two provided double operands.
    /// </summary>
    /// <param name="operand1Value">The first operand in the arithmetic operation.</param>
    /// <param name="operand2Value">The second operand in the arithmetic operation.</param>
    /// <returns>The result of the arithmetic operation as an implementation of <see cref="IParseResult{TValue}"/>.</returns>
    protected abstract IParseResult<object?> Calculate(double operand1Value, double operand2Value);
}