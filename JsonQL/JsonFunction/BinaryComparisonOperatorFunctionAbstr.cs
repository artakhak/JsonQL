using OROptimizer.Diagnostics.Log;
using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction;

/// <summary>
/// Base class for binary comparison operators such as "==", "!=", ">", ">=", "&lt;", "&lt;=", "LIKE" etc.
/// </summary>
public abstract class BinaryComparisonOperatorFunctionAbstr : BooleanJsonFunctionAbstr
{
    protected BinaryComparisonOperatorFunctionAbstr(string operatorName,
        IJsonFunction operand1, IJsonFunction operand2, IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }

    protected IJsonFunction Operand1 { get; }
    protected IJsonFunction Operand2 { get; }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var result1 = Operand1.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result1.Errors.Count > 0)
            return new ParseResult<bool?>(result1.Errors);

        var result2 = Operand2.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result2.Errors.Count > 0)
            return new ParseResult<bool?>(result2.Errors);

        JsonFunctionHelpers.TryConvertValueToJsonComparable(result1.Value, null, out var jsonComparable1);
        JsonFunctionHelpers.TryConvertValueToJsonComparable(result2.Value, null, out var jsonComparable2);

        try
        {
            return DoEvaluate(jsonComparable1, jsonComparable2);
        }
        catch (Exception e)
        {
            LogHelper.Context.Log.Error(e);
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError($"Evaluation of operator [{this.FunctionName}] failed.", this.LineInfo)));
        }
    }

    /// <summary>
    /// Compares values in <param name="evaluatedValueOfOperand1"></param> and <param name="evaluatedValueOfOperand2"></param>.
    /// </summary>
    /// <param name="evaluatedValueOfOperand1">The first compared value.</param>
    /// <param name="evaluatedValueOfOperand2">The second compared value.</param>
    /// <returns></returns>
    protected abstract IParseResult<bool?> DoEvaluate(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2);
}