using JsonQL.Compilation.JsonFunction.JsonFunctions;

namespace JsonQL.Compilation.JsonFunction;

public interface IUniversalLambdaFunction : ILambdaFunction<IJsonFunction>
{

}

/// <inheritdoc />
public class UniversalLambdaFunction : IUniversalLambdaFunction
{
    public UniversalLambdaFunction(ILambdaFunctionParameterJsonFunction parameterJsonFunction, IJsonFunction lambdaExpressionFunction)
    {
        ParameterJsonFunction = parameterJsonFunction;
        LambdaExpressionFunction = lambdaExpressionFunction;
    }

    /// <inheritdoc />
    public ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }

    /// <inheritdoc />
    public IJsonFunction LambdaExpressionFunction { get; }
}