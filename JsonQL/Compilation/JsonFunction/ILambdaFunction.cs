using JsonQL.Compilation.JsonFunction.JsonFunctions;

namespace JsonQL.Compilation.JsonFunction;

public interface ILambdaFunction<out TJsonFunction> where TJsonFunction : IJsonFunction
{
    ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }
    TJsonFunction LambdaExpressionFunction { get; }
}