using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// A function used to parse lambda expressions parameters that have any meaning in the context of
/// currently evaluated function.
/// </summary>
public interface ILambdaFunctionParameterJsonFunction : IVariableJsonFunction
{
    
}

public class LambdaFunctionParameterJsonFunction : JsonFunctionAbstr, ILambdaFunctionParameterJsonFunction
{
    public LambdaFunctionParameterJsonFunction(string parameterName, IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(parameterName, jsonFunctionContext, lineInfo)
    {
        Name = parameterName;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, 
        IJsonFunctionEvaluationContextData? contextData)
    {
        var parameterValueResult = JsonFunctionValueEvaluationContext.VariablesManager.TryEvaluateVariableValue(this.Name, contextData);

        if (parameterValueResult == null)
            return new ParseResult<object?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("Failed to evaluate parameter value", LineInfo)));

        return parameterValueResult;
    }

    /// <inheritdoc />
    public string Name { get; }
}