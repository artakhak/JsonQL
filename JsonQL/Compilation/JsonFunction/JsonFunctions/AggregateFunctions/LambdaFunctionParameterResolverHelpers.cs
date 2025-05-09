// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Provides helper methods for resolving Lambda function parameter values within the context
/// of evaluating JSON functions and variable bindings.
/// </summary>
public static class LambdaFunctionParameterResolverHelpers
{
    /// <summary>
    /// Attempts to evaluate the parameter value of a lambda function based on the provided variable name and context data.
    /// </summary>
    /// <typeparam name="TJsonFunction">The type of the JSON function used within the lambda function.</typeparam>
    /// <param name="lambdaFunctionData">The lambda function data containing the parameter to be evaluated.</param>
    /// <param name="variableName">The name of the variable whose value is to be evaluated.</param>
    /// <param name="contextData">The evaluation context data to assist in resolving the value of the variable.</param>
    /// <returns>
    /// A parse result containing the resolved value of the variable, or null if the variable name
    /// does not match the parameter name of the lambda function or if the context data is not provided.
    /// </returns>
    public static IParseResult<object?>? TryEvaluateLambdaFunctionParameterValue<TJsonFunction>(ILambdaFunction<TJsonFunction>? lambdaFunctionData, string variableName,
        IJsonFunctionEvaluationContextData? contextData) where TJsonFunction : IJsonFunction
    {
        if (lambdaFunctionData?.ParameterJsonFunction.Name != variableName)
            return null;

        if (contextData == null)
        {
            return new ParseResult<object?>(
                CollectionExpressionHelpers.Create(
                    new JsonObjectParseError("The context data was not set.", lambdaFunctionData.LambdaExpressionFunction.LineInfo)));
        }

        return new ParseResult<object?>(contextData.EvaluatedValue);
    }
}