using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function specialized in handling parameters of lambda expressions within the context
/// of JSON functions. This interface provides a mechanism to interpret and process lambda parameter
/// information in the context of evaluating JSON-based queries or expressions.
/// </summary>
public interface ILambdaFunctionParameterJsonFunction : IVariableJsonFunction
{

}

/// <summary>
/// Represents a JSON function designed to evaluate the value of a parameter passed to a lambda expression
/// within the context of a JSON function evaluation. This class ensures proper handling of variable evaluation
/// in lambda expressions by accessing the context's variable manager.
/// </summary>
public class LambdaFunctionParameterJsonFunction : JsonFunctionAbstr, ILambdaFunctionParameterJsonFunction
{
    /// <summary>
    /// Represents a JSON function parameter which acts as a placeholder for a value in a lambda expression.
    /// </summary>
    /// <remarks>
    /// This class is used to define a parameter for lambda-based expressions in JSON querying. It inherits from the
    /// <see cref="JsonFunctionAbstr"/> base class and is instantiated with a parameter name and context.
    /// </remarks>
    public LambdaFunctionParameterJsonFunction(string parameterName,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(parameterName, jsonFunctionContext, lineInfo)
    {
        Name = parameterName;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData)
    {
        var variableValue = JsonFunctionValueEvaluationContext.VariablesManager.TryResolveVariableValue(this.Name);

        if (variableValue != null)
            return variableValue;

        return new ParseResult<object?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("Failed to evaluate parameter value", LineInfo)));
    }

    /// <inheritdoc />
    public string Name { get; }
}