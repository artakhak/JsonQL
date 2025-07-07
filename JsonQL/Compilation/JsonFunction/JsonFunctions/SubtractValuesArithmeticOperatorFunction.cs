// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON-QL arithmetic function that performs subtraction between two numeric operands.
/// </summary>
/// <remarks>
/// This class is derived from <c>BinaryNumericArithmeticOperationOperatorFunctionAbstr</c>.
/// It encapsulates the logic required to subtract two numeric values as part of a JSON function evaluation.
/// </remarks>
/// <remarks>
/// The subtraction operation is carried out by overriding the <c>Calculate</c> method, where the two operand values are subtracted, and the result is returned as a parse result.
/// </remarks>
public class SubtractValuesArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a JSON subtract operator function used to perform subtraction between two numeric operands in JSON function execution.
    /// </summary>
    /// <param name="operatorName">Operator name.</param>
    /// <param name="operand1">The first operand in the subtraction operation.</param>
    /// <param name="operand2">The second operand in the subtraction operation.</param>
    /// <param name="jsonFunctionContext">The evaluation context required for JSON function execution.</param>
    /// <param name="lineInfo">Optional line information for debugging or traceability in JSON expressions.</param>
    /// <remarks>
    /// Inherits from <see cref="BinaryNumericArithmeticOperationOperatorFunctionAbstr"/> and is designed specifically for subtraction
    /// using the operator defined in <see cref="JsonOperatorNames.SubtractOperator"/>.
    /// </remarks>
    public SubtractValuesArithmeticOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<object?> Calculate(double operand1Value, double operand2Value)
    {
        return new ParseResult<object?>(operand1Value - operand2Value);
    }
}