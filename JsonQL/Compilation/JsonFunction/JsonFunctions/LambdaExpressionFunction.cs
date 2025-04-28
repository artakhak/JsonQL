using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a lambda expression function that can be evaluated within a JSON-based query language.
/// This class provides the ability to define and handle lambda expressions with parameters and an associated expression.
/// </summary>
public class LambdaExpressionFunction : JsonFunctionAbstr, ILambdaExpressionFunction
{
    /// <summary>
    /// Represents a function defined by a lambda expression. This class enables the creation and evaluation of functions based on custom lambda expressions in a JSON function context. It includes the function's name, input parameters, the lambda expression, and the evaluation context.
    /// </summary>
    /// <param name="functionName">The name of the lambda expression function.</param>
    /// <param name="parameters">The list of parameters for the lambda expression function.</param>
    /// <param name="expression">The lambda expression to be evaluated for this function.</param>
    /// <param name="jsonFunctionContext">The context in which the function is evaluated, providing execution context and state.</param>
    /// <param name="lineInfo">Optional line information for debugging and error reporting.</param>
    public LambdaExpressionFunction(string functionName,
        IReadOnlyList<ILambdaFunctionParameterJsonFunction> parameters,
        IJsonFunction expression,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        Parameters = parameters;
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