using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class BinaryNonEqualityComparisonOperatorFunction : BinaryComparisonOperatorFunctionAbstr
{
    private readonly BinaryNonEqualityComparisonOperatorType _binaryNonEqualityComparisonOperatorType;

    public BinaryNonEqualityComparisonOperatorFunction(string operatorName, BinaryNonEqualityComparisonOperatorType binaryNonEqualityComparisonOperatorType,
        IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName,
        operand1, operand2, jsonFunctionContext, lineInfo)
    {
        _binaryNonEqualityComparisonOperatorType = binaryNonEqualityComparisonOperatorType;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluate(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2)
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