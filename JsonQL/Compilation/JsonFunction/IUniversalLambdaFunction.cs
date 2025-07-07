// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.JsonFunctions;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents a universal lambda function that integrates with the JSON function evaluation framework.
/// This interface extends the capabilities of <see cref="ILambdaFunction{TJsonFunction}"/> for more
/// generic operations on JSON-based lambda expressions.
/// </summary>
public interface IUniversalLambdaFunction : ILambdaFunction<IJsonFunction>
{

}

/// <inheritdoc />
public class UniversalLambdaFunction : IUniversalLambdaFunction
{
    /// <summary>
    /// Represents a universal lambda function that is used within JSON function evaluation contexts.
    /// It is composed of a parameter JSON function and a lambda expression function.
    /// </summary>
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