// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function that performs multiplication of two numeric values
/// within the context of JSON function execution.
/// </summary>
public class MultiplyValuesArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a function that performs multiplication on two numeric operands using the multiply operator.
    /// </summary>
    /// <param name="operatorName">Operator name.</param>
    /// <param name="operand1">The first operand to be multiplied.</param>
    /// <param name="operand2">The second operand to be multiplied.</param>
    /// <param name="jsonFunctionContext">The context used for evaluating the JSON function.</param>
    /// <param name="lineInfo">Optional line information for error tracing or debugging.</param>
    /// <remarks>
    /// This class inherits from <see cref="BinaryNumericArithmeticOperationOperatorFunctionAbstr"/>
    /// and leverages the multiply operator implementation defined in <see cref="JsonOperatorNames.MultiplyOperator"/>.
    /// </remarks>
    public MultiplyValuesArithmeticOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<double?> Calculate(IJsonComparable operand1Value, IJsonComparable operand2Value)
    {
        if (operand1Value.Value is not double operand1DoubleValue || operand2Value.Value is not double operand2DoubleValue)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>(operand1DoubleValue * operand2DoubleValue);
    }
}
