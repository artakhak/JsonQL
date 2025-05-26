using JsonQL.Compilation.JsonFunction;
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
    protected override IParseResult<object?> Calculate(double operand1Value, double operand2Value)
    {
        return new ParseResult<object?>(-(operand1Value + operand2Value));
    }
}