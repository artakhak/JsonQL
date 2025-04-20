using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction;

/// <inheritdoc />
public abstract class JsonFunctionAbstr : IJsonFunction
{
    protected JsonFunctionAbstr(string functionName,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        FunctionName = functionName;
        JsonFunctionValueEvaluationContext = jsonFunctionContext;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public string FunctionName { get; }
    
    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc />
    public IJsonFunction? ParentJsonFunction => JsonFunctionValueEvaluationContext.ParentJsonFunction;

    protected IJsonFunctionValueEvaluationContext JsonFunctionValueEvaluationContext { get; }
    
    /// <inheritdoc />
    public IParseResult<object?> EvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData)
    {
        IResolvesVariableValue? resolvesVariableValue = this as IResolvesVariableValue;

        if (resolvesVariableValue != null)
            this.JsonFunctionValueEvaluationContext.VariablesManager.Register(resolvesVariableValue);

        var result = DoEvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (resolvesVariableValue != null)
            this.JsonFunctionValueEvaluationContext.VariablesManager.UnRegister(resolvesVariableValue);

        return result;
    }

    protected abstract IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData);
}