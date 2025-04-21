using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class BinaryNotEqualsOperatorFunction : BinaryComparisonOperatorFunctionAbstr
{
    public BinaryNotEqualsOperatorFunction(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluate(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2)
    {
        if (evaluatedValueOfOperand1 == null || evaluatedValueOfOperand2 == null)
            return new ParseResult<bool?>(true);

        if (evaluatedValueOfOperand1.TypeCode != evaluatedValueOfOperand2.TypeCode)
            return new ParseResult<bool?>(true);

        return new ParseResult<bool?>(!object.Equals(evaluatedValueOfOperand1.Value, evaluatedValueOfOperand2.Value));
    }
}