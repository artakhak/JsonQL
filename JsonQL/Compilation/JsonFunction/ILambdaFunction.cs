// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Represents a generic lambda function interface that operates on JSON-based functions.
/// This interface provides abstraction for handling both parameterized JSON functions and
/// the lambda expression function itself.
/// </summary>
/// <typeparam name="TJsonFunction">
/// The type of JSON function associated with this lambda function. Must implement <see cref="IJsonFunction"/>.
/// </typeparam>
public interface ILambdaFunction<out TJsonFunction> where TJsonFunction : IJsonFunction
{
    /// <summary>
    /// Gets the component responsible for managing and parsing parameters in a lambda function that
    /// operates within a JSON processing context. This property facilitates handling the parameters
    /// used in lambda expressions and ensures their correct evaluation or interpretation.
    /// </summary>
    /// <remarks>
    /// The <see cref="ParameterJsonFunction"/> is typically utilized in scenarios where lambda function
    /// parameters are parsed or evaluated, contributing to the resolution of parameterized JSON functions.
    /// It implements the <see cref="ILambdaFunctionParameterJsonFunction"/> interface, which provides
    /// additional capabilities specific to lambda parameter handling.
    /// </remarks>
    ILambdaFunctionParameterJsonFunction ParameterJsonFunction { get; }

    /// <summary>
    /// Represents the lambda expression assigned to a function, enabling the evaluation or execution
    /// of specific logic within JSON processing contexts. This property facilitates the encapsulation
    /// of functionality required for various lambda operations like predicate checks, path resolution,
    /// or value transformations.
    /// </summary>
    /// <remarks>
    /// The <see cref="LambdaExpressionFunction"/> provides access to an interface or implementation
    /// that defines specific behavior depending on the context. Different implementations, such as
    /// <see cref="IBooleanJsonFunction"/> for predicate operations or <see cref="IJsonValuePathJsonFunction"/>
    /// for path evaluations, allow tailored execution strategies within JSON-related logic flows.
    /// </remarks>
    TJsonFunction LambdaExpressionFunction { get; }
}