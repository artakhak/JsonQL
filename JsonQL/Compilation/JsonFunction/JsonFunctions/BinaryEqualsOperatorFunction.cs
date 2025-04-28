using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a binary equals (==) operator function in the JsonQL compilation framework.
/// This class evaluates whether the values of two operands are equal.
/// The class inherits from <see cref="BinaryComparisonOperatorFunctionAbstr"/> and leverages the framework's logic
/// to perform comparisons between JSON values, with support for type checking and null handling.
/// </summary>
public class BinaryEqualsOperatorFunction : BinaryComparisonOperatorFunctionAbstr
{
    /// <summary>
    /// Defines a binary comparison function that checks for equality between two operands
    /// within the context of a JSON function evaluation.
    /// </summary>
    /// <param name="operatorName">The name of the operator used for the comparison.</param>
    /// <param name="operand1">The first operand involved in the comparison operation.</param>
    /// <param name="operand2">The second operand involved in the comparison operation.</param>
    /// <param name="jsonFunctionContext">The context used to evaluate the values of the operands.</param>
    /// <param name="lineInfo">Optional line information for debugging or error reporting purposes.</param>
    public BinaryEqualsOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluate(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2)
    {
        if (evaluatedValueOfOperand1 == null || evaluatedValueOfOperand2 == null)
            return new ParseResult<bool?>(false);

        if (evaluatedValueOfOperand1.TypeCode != evaluatedValueOfOperand2.TypeCode)
            return new ParseResult<bool?>(false);

        return new ParseResult<bool?>(Equals(evaluatedValueOfOperand1.Value, evaluatedValueOfOperand2.Value));
    }
}