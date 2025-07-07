// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a lambda expression function within the JsonQL framework.
/// This interface defines the structure of a lambda expression, including its parameters and body expression.
/// </summary>
public interface ILambdaExpressionFunction : IJsonFunction
{
    /// <summary>
    /// Gets the list of parameters for the lambda expression function.
    /// Each parameter is represented as an instance of <see cref="ILambdaFunctionParameterJsonFunction"/>,
    /// providing metadata and functionality related to the individual parameter objects of the lambda expression.
    /// </summary>
    IReadOnlyList<ILambdaFunctionParameterJsonFunction> Parameters { get; }

    /// <summary>
    /// Gets the body of the lambda expression function.
    /// The body is represented as an instance of <see cref="IJsonFunction"/>,
    /// which provides the logic or functional operation performed by the lambda expression.
    /// </summary>
    IJsonFunction Expression { get; }
}