using JsonQL.Compilation.JsonFunction.JsonFunctions;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents a specialized lambda function designed to work with JSON path-based operations.
/// This interface extends the generic lambda function interface and utilizes JSON path functions
/// as its primary expression and parameter representations for evaluating and processing JSON data structures.
/// </summary>
/// <remarks>
/// This interface is typically used in scenarios where JSON path syntax and lambda functions
/// are combined to filter, map, or process JSON data based on dynamic criteria defined at runtime.
/// </remarks>
public interface IJsonPathLambdaFunction: ILambdaFunction<IJsonValuePathJsonFunction>
{
    
}

/// <inheritdoc />
public class JsonPathLambdaFunction : IJsonPathLambdaFunction
{
    /// <summary>
    /// Represents a lambda function that combines a parameter JSON function
    /// and a JSON value path function to evaluate JSON expressions
    /// against a specified value path in the JSON structure.
    /// </summary>
    public JsonPathLambdaFunction(ILambdaFunctionParameterJsonFunction parameterJsonFunction, IJsonValuePathJsonFunction jsonValuePathJsonFunction)
    {
        ParameterJsonFunction = parameterJsonFunction;
        LambdaExpressionFunction = jsonValuePathJsonFunction;
    }

    /// <inheritdoc />
    public ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }

    /// <inheritdoc />
    public IJsonValuePathJsonFunction LambdaExpressionFunction { get; }
}