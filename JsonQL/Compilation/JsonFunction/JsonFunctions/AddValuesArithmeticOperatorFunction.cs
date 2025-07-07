// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a concrete implementation of a binary numeric arithmetic operator that performs addition.
/// </summary>
/// <remarks>
/// This class is part of the arithmetic operator functions hierarchy and is designed to handle the addition of
/// numeric values in a context where JSON-like data structures and functions are being utilized. It extends
/// <see cref="BinaryNumericArithmeticOperationOperatorFunctionAbstr"/>, thereby inheriting functionality for
/// handling binary arithmetic operations generally, and specializes in the add operator.
/// </remarks>
public class AddValuesArithmeticOperatorFunction : BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a function that performs addition operation on two numeric operands.
    /// </summary>
    /// <param name="operatorName">Operator name.</param>
    /// <param name="operand1">The first operand of the addition operation.</param>
    /// <param name="operand2">The second operand of the addition operation.</param>
    /// <param name="jsonFunctionContext">The context that defines the evaluation settings for the function.</param>
    /// <param name="lineInfo">Optional line information for error reporting or debugging purposes.</param>
    public AddValuesArithmeticOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<object?> Calculate(double operand1Value, double operand2Value)
    {
        return new ParseResult<object?>(operand1Value + operand2Value);
    }
}