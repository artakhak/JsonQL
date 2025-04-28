using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function that evaluates whether a string contains another string within the context of JSON-based expressions.
/// </summary>
/// <remarks>
/// This class inherits from <see cref="BinaryStringComparisonOperatorFunctionAbstr"/> and implements the logic for the
/// "contains" operation between two string operands.
/// </remarks>
/// <example>
/// Operates by checking if the first string operand contains the second string operand. Returns a boolean result.
/// If either operand is null, it evaluates to false.
/// </example>
public class ContainsOperatorFunction: BinaryStringComparisonOperatorFunctionAbstr
{
    /// <summary>
    /// Implements the "Contains" logical operator for evaluating whether one string operand contains another
    /// in the context of JSON function operations.
    /// </summary>
    /// <param name="operatorName">The name of the operator, typically "Contains".</param>
    /// <param name="operand1">The first operand for the comparison, representing the string to be checked.</param>
    /// <param name="operand2">The second operand for the comparison, representing the string to look for.</param>
    /// <param name="jsonFunctionContext">The evaluation context for JSON functions.</param>
    /// <param name="lineInfo">Optional information about the line in the source for debugging or error reporting purposes.</param>
    /// <remarks>
    /// This class extends an abstract base class designed for binary string comparison operations, enabling custom logic
    /// for "Contains" operator evaluations in a structured JSON function environment.
    /// </remarks>
    public ContainsOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
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

        return new ParseResult<bool?>(operand1Value.Contains(operand2Value));
    }
}