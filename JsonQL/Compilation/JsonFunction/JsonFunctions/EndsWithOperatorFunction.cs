// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function that evaluates whether the first string operand ends with the second string operand.
/// </summary>
/// <remarks>
/// This class is a specific implementation of a binary string comparison function.
/// It is primarily used within the context of JSON function evaluations.
/// </remarks>
public class EndsWithOperatorFunction : BinaryStringComparisonOperatorFunctionAbstr
{
    /// <summary>
    /// Represents a JSON function for evaluating whether a string ends with another string within the JSON query language execution context.
    /// </summary>
    /// <param name="operatorName">The name of the operator associated with the function.</param>
    /// <param name="operand1">The first operand used in the comparison, representing the target string.</param>
    /// <param name="operand2">The second operand used in the comparison, representing the substring to match at the end of the target string.</param>
    /// <param name="jsonFunctionContext">The execution context for JSON function evaluation.</param>
    /// <param name="lineInfo">Optional information about the line and position of the function in the source JSON query.</param>
    /// <remarks>
    /// Inherits from <see cref="BinaryStringComparisonOperatorFunctionAbstr"/>, providing specific implementation for the "EndsWith" string comparison operation in JSON queries.
    /// </remarks>
    public EndsWithOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> Calculate(string? operand1Value, string? operand2Value)
    {
        if (operand1Value == null || operand2Value == null)
            return new ParseResult<bool?>(false);

        return new ParseResult<bool?>(operand1Value.EndsWith(operand2Value));
    }
}
