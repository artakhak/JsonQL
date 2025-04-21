using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonFunction;

public abstract class BinaryNumericArithmeticOperationOperatorFunctionAbstr : BinaryArithmeticOperationOperatorFunctionAbstr
{
    protected BinaryNumericArithmeticOperationOperatorFunctionAbstr(string operatorName, IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) :
        base(operatorName, operand1, operand2, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public sealed override IParseResult<object?> Calculate(IJsonComparable operand1Value, IJsonComparable operand2Value)
    {
        //string GetInvalidOperandTypeErrorMessage(string operandName) => $"{operandName} is not a numeric value";

        //if (operand1Value.Value is not double operand1DoubleValue)
        //    return new ParseResult<object?>([new JsonObjectParseError(GetInvalidOperandTypeErrorMessage("Operand 1"), Operand1.LineInfo)]);

        //if (operand2Value.Value is not double operand2DoubleValue)
        //    return new ParseResult<object?>([new JsonObjectParseError(GetInvalidOperandTypeErrorMessage("Operand 2"), Operand2.LineInfo)]);

        if (operand1Value.Value is not double operand1DoubleValue || operand2Value.Value is not double operand2DoubleValue)
            return new ParseResult<object?>((bool?)null);

        try
        {
            return Calculate(operand1DoubleValue, operand2DoubleValue);
        }
        catch (Exception e) // OverflowException
        {
            var errorMessage = $"Failed to evaluate the operator [{this.FunctionName}] for values [{operand1Value}] and [{operand2Value}].";
            LogHelper.Context.Log.Error(errorMessage, e);
            return new ParseResult<object?>(CollectionExpressionHelpers.Create(new JsonObjectParseError(errorMessage, this.LineInfo)));
        }
    }

    protected abstract IParseResult<object?> Calculate(double operand1Value, double operand2Value);
}