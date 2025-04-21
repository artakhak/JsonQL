using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class LambdaExpressionFunction : JsonFunctionAbstr, ILambdaExpressionFunction
{
    public LambdaExpressionFunction(string functionName,
        IReadOnlyList<ILambdaFunctionParameterJsonFunction> parameters,
        IJsonFunction expression,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        Parameters = parameters;
        //_variablesStore = new VariablesStore(Parameters);
        Expression = expression;
    }

    /// <inheritdoc />
    public IReadOnlyList<ILambdaFunctionParameterJsonFunction> Parameters { get; }

    /// <inheritdoc />
    public IJsonFunction Expression { get; }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.Expression.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);
    }
}