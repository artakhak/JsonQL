using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a binary logical OR operator function that evaluates logical expressions.
/// </summary>
/// <remarks>
/// This class is derived from the <c>BinaryLogicalOperatorFunctionAbstr</c> base class
/// and implements the evaluation of a binary logical OR operation between two operands.
/// </remarks>
public class BinaryOrLogicalOperatorFunction : BinaryLogicalOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a logical operator function that performs a binary OR operation.
    /// </summary>
    /// <param name="operatorName">The name of the operator.</param>
    /// <param name="operand1">The first operand of the binary OR operation.</param>
    /// <param name="operand2">The second operand of the binary OR operation.</param>
    /// <param name="jsonFunctionContext">The context used for JSON function evaluation.</param>
    /// <param name="lineInfo">Optional line information for error or logging purposes.</param>
    public BinaryOrLogicalOperatorFunction(string operatorName, IBooleanJsonFunction operand1, IBooleanJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluateBooleanValue(bool? evaluatedValueOfOperand1, bool? evaluatedValueOfOperand2)
    {
        return new ParseResult<bool?>(evaluatedValueOfOperand1 != null && evaluatedValueOfOperand1.Value ||
                                      evaluatedValueOfOperand2 != null && evaluatedValueOfOperand2.Value);
    }
}