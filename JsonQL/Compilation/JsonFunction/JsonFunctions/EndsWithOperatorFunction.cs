using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class EndsWithOperatorFunction : BinaryStringComparisonOperatorFunctionAbstr
{
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