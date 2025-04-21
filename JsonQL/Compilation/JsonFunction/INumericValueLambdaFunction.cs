using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;

namespace JsonQL.Compilation.JsonFunction;

public interface INumericValueLambdaFunction : ILambdaFunction<IDoubleJsonFunction>
{

}

public class NumericValueLambdaFunction : INumericValueLambdaFunction
{
    public NumericValueLambdaFunction(ILambdaFunctionParameterJsonFunction parameterJsonFunction, IDoubleJsonFunction doubleJsonFunction)
    {
        ParameterJsonFunction = parameterJsonFunction;
        LambdaExpressionFunction = doubleJsonFunction;
    }

    /// <inheritdoc />
    public ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }

    /// <inheritdoc />
    public IDoubleJsonFunction LambdaExpressionFunction { get; }
}