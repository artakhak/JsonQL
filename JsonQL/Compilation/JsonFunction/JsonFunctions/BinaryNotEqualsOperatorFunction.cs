// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a binary operator function that evaluates the inequality between two operands.
/// This function checks whether two JSON-comparable operands are not equal in value.
/// </summary>
public class BinaryNotEqualsOperatorFunction : BinaryComparisonOperatorFunctionAbstr
{
    /// <summary>
    /// Defines a function for performing the binary not-equals comparison operation between two JSON function operands.
    /// </summary>
    /// <param name="operatorName">The binary operator's name, typically representing inequality.</param>
    /// <param name="operand1">The first operand of the not-equals comparison.</param>
    /// <param name="operand2">The second operand of the not-equals comparison.</param>
    /// <param name="jsonFunctionContext">Provides the evaluation context in which the function operates.</param>
    /// <param name="lineInfo">Optional line information for debugging or error tracing.</param>
    /// <remarks>
    /// This class inherits from the abstract binary comparison operator function and evaluates whether the two operands are not equal.
    /// </remarks>
    public BinaryNotEqualsOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluateBooleanValue(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2)
    {
        if (evaluatedValueOfOperand1 == null || evaluatedValueOfOperand2 == null)
            return new ParseResult<bool?>(true);

        if (evaluatedValueOfOperand1.TypeCode != evaluatedValueOfOperand2.TypeCode)
            return new ParseResult<bool?>(true);

        return new ParseResult<bool?>(!Equals(evaluatedValueOfOperand1.Value, evaluatedValueOfOperand2.Value));
    }
}