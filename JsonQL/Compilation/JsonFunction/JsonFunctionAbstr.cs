using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

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

    /// <summary>
    /// Represents a context that encapsulates the evaluation of function values
    /// within JSON processing, providing access to parent functions and variable management.
    /// </summary>
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

    /// <summary>
    /// Evaluates a value based on the provided parsed value, parent root parsed values, and context data.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value to base the evaluation on.</param>
    /// <param name="compiledParentRootParsedValues">A collection of parent root parsed values used during evaluation.</param>
    /// <param name="contextData">Optional context data used during the evaluation.</param>
    /// <returns>A result of the evaluation encapsulated in an <see cref="IParseResult{TValue}" /> instance.</returns>
    protected abstract IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData);
}