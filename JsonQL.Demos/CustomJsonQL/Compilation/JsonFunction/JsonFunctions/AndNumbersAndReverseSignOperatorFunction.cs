using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;

/// <inheritdoc />
public class AndNumbersAndReverseSignOperatorFunction: BinaryNumericArithmeticOperationOperatorFunctionAbstr
{
    public AndNumbersAndReverseSignOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<double?> Calculate(IJsonComparable operand1Value, IJsonComparable operand2Value)
    {
        if (operand1Value.Value is not double operand1DoubleValue || operand2Value.Value is not double operand2DoubleValue)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>(-(operand1DoubleValue + operand2DoubleValue));
    }
}
