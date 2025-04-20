using JsonQL.JsonFunction.JsonFunctions;

namespace JsonQL.JsonFunction;

public interface ILambdaFunction<out TJsonFunction> where TJsonFunction : IJsonFunction
{
    ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }
    TJsonFunction LambdaExpressionFunction { get; }
}