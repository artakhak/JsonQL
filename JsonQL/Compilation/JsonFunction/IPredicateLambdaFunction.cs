using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;

namespace JsonQL.Compilation.JsonFunction;

public interface IPredicateLambdaFunction: ILambdaFunction<IBooleanJsonFunction>
{
   
}

public class PredicateLambdaFunction : IPredicateLambdaFunction
{
    public PredicateLambdaFunction(ILambdaFunctionParameterJsonFunction parameterJsonFunction, IBooleanJsonFunction predicate)
    {
        ParameterJsonFunction = parameterJsonFunction;
        LambdaExpressionFunction = predicate;
    }

    /// <inheritdoc />
    public ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }

    /// <inheritdoc />
    public IBooleanJsonFunction LambdaExpressionFunction { get; }
}