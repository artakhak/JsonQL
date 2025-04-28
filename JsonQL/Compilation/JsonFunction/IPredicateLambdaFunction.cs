using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents a specialized lambda function interface where the return type is restricted to boolean logic.
/// Implementations of this interface define a predicate using a parameter function and a boolean expression function.
/// This interface is commonly utilized in contexts where conditional evaluation or filtering is required.
/// </summary>
public interface IPredicateLambdaFunction: ILambdaFunction<IBooleanJsonFunction>
{
   
}

/// <inheritdoc />
public class PredicateLambdaFunction : IPredicateLambdaFunction
{
    /// <summary>
    /// Represents a lambda function that takes parameters and a boolean predicate, intended for use in evaluating JSON-related functionality.
    /// </summary>
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