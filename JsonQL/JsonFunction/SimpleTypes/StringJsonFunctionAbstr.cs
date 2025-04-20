using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.SimpleTypes;

public abstract class StringJsonFunctionAbstr : JsonFunctionAbstr, IStringJsonFunction
{
    protected StringJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        
    }

    /// <inheritdoc />
    public IParseResult<string?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.DoEvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToString(this.LineInfo);
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.GetStringValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    protected abstract IParseResult<string?> GetStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}