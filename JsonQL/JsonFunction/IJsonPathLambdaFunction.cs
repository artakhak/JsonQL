using JsonQL.JsonFunction.JsonFunctions;

namespace JsonQL.JsonFunction;

public interface IJsonPathLambdaFunction: ILambdaFunction<IJsonValuePathJsonFunction>
{
    
}

public class JsonPathLambdaFunction : IJsonPathLambdaFunction
{
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