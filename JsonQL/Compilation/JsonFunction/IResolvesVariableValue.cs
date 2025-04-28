namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents an interface that defines a mechanism for resolving the value of a variable.
/// </summary>
public interface IResolvesVariableValue
{
    /// <summary>
    /// Attempts to evaluate the value of a given variable within a specific context.
    /// </summary>
    /// <param name="variableName">The name of the variable to be evaluated.</param>
    /// <param name="contextData">The context data that may be used to resolve the variable value.</param>
    /// <returns>An instance of <see cref="IParseResult{TValue}"/> containing the resolved value, or null if resolution fails.</returns>
    IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData);
}