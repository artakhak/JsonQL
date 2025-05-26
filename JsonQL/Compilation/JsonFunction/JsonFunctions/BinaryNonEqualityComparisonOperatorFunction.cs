using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a binary non-equality comparison operator function in the JSON Query Language.
/// This class defines the behavior and evaluation of binary expressions where the
/// comparison checks for non-equality between two operands.
/// </summary>
public class BinaryNonEqualityComparisonOperatorFunction : BinaryComparisonOperatorFunctionAbstr
{
    private readonly BinaryNonEqualityComparisonOperatorType _binaryNonEqualityComparisonOperatorType;

    /// <summary>
    /// Represents a binary non-equality comparison operator function for evaluating JSON expressions.
    /// </summary>
    /// <remarks>
    /// This function is used to compare two operands using non-equality comparison operators such as
    /// less than, less than or equal, greater than, and greater than or equal. It extends the
    /// <see cref="BinaryComparisonOperatorFunctionAbstr"/> base class and includes additional functionality
    /// specific to non-equality comparisons.
    /// </remarks>
    /// <param name="operatorName">The name of the operator (e.g., "LessThan", "GreaterThan").</param>
    /// <param name="binaryNonEqualityComparisonOperatorType">The specific type of non-equality operator used for comparison.</param>
    /// <param name="operand1">The left-hand operand of the comparison operation.</param>
    /// <param name="operand2">The right-hand operand of the comparison operation.</param>
    /// <param name="jsonFunctionContext">The context for evaluating JSON functions, providing necessary metadata.</param>
    /// <param name="lineInfo">Optional line information for debugging or error reporting purposes.</param>
    public BinaryNonEqualityComparisonOperatorFunction(string operatorName, BinaryNonEqualityComparisonOperatorType binaryNonEqualityComparisonOperatorType,
        IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName,
        operand1, operand2, jsonFunctionContext, lineInfo)
    {
        _binaryNonEqualityComparisonOperatorType = binaryNonEqualityComparisonOperatorType;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluateBooleanValue(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2)
    {
        if (evaluatedValueOfOperand1 == null || evaluatedValueOfOperand2 == null ||
            evaluatedValueOfOperand1.TypeCode != evaluatedValueOfOperand2.TypeCode)
            return new ParseResult<bool?>((bool?)null);

        var compareToResult = evaluatedValueOfOperand1.Value.CompareTo(evaluatedValueOfOperand2.Value);

        bool comparisonResult = false;
        switch (_binaryNonEqualityComparisonOperatorType)
        {
            case BinaryNonEqualityComparisonOperatorType.LessThan:
                comparisonResult = compareToResult < 0;
                break;

            case BinaryNonEqualityComparisonOperatorType.LessThanOrEqual:
                comparisonResult = compareToResult <= 0;
                break;

            case BinaryNonEqualityComparisonOperatorType.GreaterThan:
                comparisonResult = compareToResult > 0;
                break;

            case BinaryNonEqualityComparisonOperatorType.GreaterThanOrEqual:
                comparisonResult = compareToResult >= 0;
                break;
        }

        return new ParseResult<bool?>(comparisonResult);
    }
}