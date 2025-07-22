// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a binary logical operator implementation for the logical AND operation.
/// This class extends the functionality of the abstract base class
/// <see cref="BinaryLogicalOperatorFunctionAbstr"/> to specifically perform
/// a logical AND operation on two boolean operands.
/// </summary>
public class BinaryAndLogicalOperatorFunction: BinaryLogicalOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a binary logical AND operator function, inheriting from the abstract base class
    /// for binary logical operators, used within JSON evaluation contexts.
    /// </summary>
    /// <param name="operatorName">The name of the operator.</param>
    /// <param name="operand1">The first operand implementing the IBooleanJsonFunction interface.</param>
    /// <param name="operand2">The second operand implementing the IBooleanJsonFunction interface.</param>
    /// <param name="jsonFunctionContext">The evaluation context for JSON function values.</param>
    /// <param name="lineInfo">Optional line information for diagnostic purposes.</param>
    public BinaryAndLogicalOperatorFunction(string operatorName, IBooleanJsonFunction operand1, IBooleanJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluateBooleanValue(bool? evaluatedValueOfOperand1, bool? evaluatedValueOfOperand2)
    {
        if (evaluatedValueOfOperand1 == null || evaluatedValueOfOperand2 == null)
            return new ParseResult<bool?>((bool?)null);

        return new ParseResult<bool?>(evaluatedValueOfOperand1.Value && evaluatedValueOfOperand2.Value);
    }
}
