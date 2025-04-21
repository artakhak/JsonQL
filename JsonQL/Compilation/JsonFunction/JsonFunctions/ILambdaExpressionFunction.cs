namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public interface ILambdaExpressionFunction : IJsonFunction
{
    IReadOnlyList<ILambdaFunctionParameterJsonFunction> Parameters { get; }
    IJsonFunction Expression { get; }
}