using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

public abstract class DoubleJsonFunctionAbstr : JsonFunctionAbstr, IDoubleJsonFunction
{
    protected DoubleJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.DoEvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(this.LineInfo);
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.GetDoubleValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    protected abstract IParseResult<double?> GetDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}