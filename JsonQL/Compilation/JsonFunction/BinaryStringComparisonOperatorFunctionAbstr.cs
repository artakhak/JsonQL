using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonFunction;

public abstract class BinaryStringComparisonOperatorFunctionAbstr : BinaryComparisonOperatorFunctionAbstr
{
    protected BinaryStringComparisonOperatorFunctionAbstr(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> DoEvaluate(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2)
    {
        string? stringValue1 = evaluatedValueOfOperand1?.Value as string;
        string? stringValue2 = evaluatedValueOfOperand2?.Value as string;

        try
        {
            return Calculate(stringValue1, stringValue2);
        }
        catch (Exception e) // OverflowException
        {
            var errorMessage = $"Failed to evaluate the operator [{this.FunctionName}] for values [{stringValue1}] and [{stringValue2}].";
            LogHelper.Context.Log.Error(errorMessage, e);
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError(errorMessage, this.LineInfo)));
        }
    }

    protected abstract IParseResult<bool?> Calculate(string? operand1Value, string? operand2Value);
}