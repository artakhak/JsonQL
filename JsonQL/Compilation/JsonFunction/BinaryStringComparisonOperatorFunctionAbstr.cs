// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents an abstract base class for binary string comparison functions within JSON queries.
/// </summary>
/// <remarks>
/// This class extends the functionality of <see cref="BinaryComparisonOperatorFunctionAbstr"/> to specifically handle
/// string comparison operations. It provides the foundation for derived classes to implement specific comparison logic,
/// such as "contains", "starts with", or "ends with" operations.
/// </remarks>
/// <example>
/// This class should be extended to define specific binary string comparison operators.
/// Derived classes must implement the abstract <c>Calculate</c> method for their comparison logic.
/// </example>
public abstract class BinaryStringComparisonOperatorFunctionAbstr : BinaryComparisonOperatorFunctionAbstr
{
    protected BinaryStringComparisonOperatorFunctionAbstr(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected sealed override IParseResult<bool?> DoEvaluateBooleanValue(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2)
    {
        string? stringValue1 = evaluatedValueOfOperand1?.Value as string;
        string? stringValue2 = evaluatedValueOfOperand2?.Value as string;

        try
        {
            return Calculate(stringValue1, stringValue2);
        }
        catch (Exception e)
        {
            var errorMessage = $"Failed to evaluate the operator [{this.FunctionName}] for values [{stringValue1}] and [{stringValue2}].";
            ThreadStaticLoggingContext.Context.Error(errorMessage, e);
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError(errorMessage, this.LineInfo)));
        }
    }

    /// <summary>
    /// Evaluates the comparison of two string operands based on a specific operator logic.
    /// </summary>
    /// <param name="operand1Value">The first operand to be compared, or null if not provided.</param>
    /// <param name="operand2Value">The second operand to be compared, or null if not provided.</param>
    /// <returns>An instance of <see cref="IParseResult{TValue}"/> representing the evaluation result of the comparison.</returns>
    protected abstract IParseResult<bool?> Calculate(string? operand1Value, string? operand2Value);
}
