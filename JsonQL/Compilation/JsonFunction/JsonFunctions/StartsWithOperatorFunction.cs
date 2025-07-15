// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function that performs a "StartsWith" operation on two string operands.
/// </summary>
/// <remarks>
/// This class evaluates whether the first string operand starts with the second string operand.
/// It inherits from <see cref="BinaryStringComparisonOperatorFunctionAbstr"/> and implements
/// the specific logic for the "StartsWith" string comparison.
/// </remarks>
public class StartsWithOperatorFunction : BinaryStringComparisonOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a function designed to handle the "starts with" string comparison operator
    /// within the JSON query language.
    /// </summary>
    /// <param name="operatorName">The name of the operator being evaluated.</param>
    /// <param name="operand1">The first operand of the comparison operation.</param>
    /// <param name="operand2">The second operand of the comparison operation.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function's values.</param>
    /// <param name="lineInfo">Optional line information for error handling or debugging purposes.</param>
    /// <remarks>
    /// Inherits from <see cref="BinaryStringComparisonOperatorFunctionAbstr"/>, enabling
    /// evaluation of string comparison functionality for operations where the target string
    /// must start with the specified value.
    /// </remarks>
    public StartsWithOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> Calculate(string? operand1Value, string? operand2Value)
    {
        if (operand1Value == null || operand2Value == null)
            return new ParseResult<bool?>(false);

        return new ParseResult<bool?>(operand1Value.StartsWith(operand2Value));
    }
}