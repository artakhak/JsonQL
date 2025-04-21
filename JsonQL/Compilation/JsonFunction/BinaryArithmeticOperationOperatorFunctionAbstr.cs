using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Base class for arithmetic operator operations for operators such as '+', '-', '/', '*', etc.
/// </summary>
public abstract class BinaryArithmeticOperationOperatorFunctionAbstr : JsonFunctionAbstr
{
    protected BinaryArithmeticOperationOperatorFunctionAbstr(string operatorName,
        IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }

    protected IJsonFunction Operand1 { get; }
    protected IJsonFunction Operand2 { get; }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    { 
        var evaluatedOperand1ValueResult = Operand1.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedOperand1ValueResult.Errors.Count > 0)
            return new ParseResult<object?>(evaluatedOperand1ValueResult.Errors);

        var evaluatedOperand2ValueResult = Operand2.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedOperand2ValueResult.Errors.Count > 0)
            return new ParseResult<object?>(evaluatedOperand1ValueResult.Errors);
            
        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedOperand1ValueResult.Value, null, out var jsonComparable1) ||
            !JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedOperand2ValueResult.Value, null, out var jsonComparable2))
        {
            return new ParseResult<object?>((object?) null);
        }

        return Calculate(jsonComparable1, jsonComparable2);
    }
    
    public abstract IParseResult<object?> Calculate(IJsonComparable operand1Value, IJsonComparable operand2Value);
}