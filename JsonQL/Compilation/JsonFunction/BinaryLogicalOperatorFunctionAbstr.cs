using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// A function for binary boolean operators such as "&amp;", "||", etc.
/// </summary>
public abstract class BinaryLogicalOperatorFunctionAbstr : BooleanJsonFunctionAbstr
{
    protected BinaryLogicalOperatorFunctionAbstr(string operatorName,
        IBooleanJsonFunction operand1, IBooleanJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }

    protected IBooleanJsonFunction Operand1 { get; }
    protected IBooleanJsonFunction Operand2 { get; }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var result1 = Operand1.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result1.Errors.Count > 0)
            return new ParseResult<bool?>(result1.Errors);

        var result2 = Operand2.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result2.Errors.Count > 0)
            return new ParseResult<bool?>(result2.Errors);

        return DoEvaluate(result1.Value, result2.Value);
    }

    protected abstract IParseResult<bool?> DoEvaluate(bool? evaluatedValueOfOperand1, bool? evaluatedValueOfOperand2);
}