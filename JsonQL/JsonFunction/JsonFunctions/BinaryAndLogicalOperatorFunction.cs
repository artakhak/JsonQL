using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class BinaryAndLogicalOperatorFunction: BinaryLogicalOperatorFunctionAbstr
{
    public BinaryAndLogicalOperatorFunction(string operatorName, IBooleanJsonFunction operand1, IBooleanJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : 
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluate(bool? evaluatedValueOfOperand1, bool? evaluatedValueOfOperand2)
    {
        if (evaluatedValueOfOperand1 == null || evaluatedValueOfOperand2 == null)
            return new ParseResult<bool?>((bool?)null);

        return new ParseResult<bool?>(evaluatedValueOfOperand1.Value && evaluatedValueOfOperand2.Value);
    }
}