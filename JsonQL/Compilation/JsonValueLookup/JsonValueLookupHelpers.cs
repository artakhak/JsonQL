// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Helper methods that can be used in implementations of <see cref="IJsonValueCollectionItemsSelectorPathElementFactory"/>
/// </summary>
public static class JsonValueLookupHelpers
{
    /// <summary>
    /// Creates an instance of <see cref="IPredicateLambdaFunction"/> from <see cref="ILambdaExpressionFunction"/>. 
    /// </summary>
    /// <param name="selectorName">Selector name.</param>
    /// <param name="lambdaExpressionFunction">Lambda expression function.</param>
    /// <param name="lambdaPredicate">
    /// Output parameter for generated <see cref="IPredicateLambdaFunction"/>.
    /// The value is not null if the return value is true.
    /// </param>
    /// <param name="jsonObjectParseError">Error details. The value is not null if the return value is false.</param>
    /// <remarks>
    /// These methods might be modified later to support more general use cases, such as predicates with more than one parameter.
    /// Therefore, use these method with this understanding in mind.
    /// </remarks>
    public static bool TryGetLambdaPredicateFromParameter(string selectorName, ILambdaExpressionFunction lambdaExpressionFunction, [NotNullWhen(true)] out IPredicateLambdaFunction? lambdaPredicate, [NotNullWhen(false)] out JsonObjectParseError? jsonObjectParseError)
    {
        lambdaPredicate = null;
        jsonObjectParseError = null;

        if (lambdaExpressionFunction.Parameters.Count != 1)
        {
            jsonObjectParseError = new JsonObjectParseError(
                $"Lambda expression parameter in function [{selectorName}] is expected to have one parameter.",
                lambdaExpressionFunction.Parameters.Count == 0 ? lambdaExpressionFunction.LineInfo : lambdaExpressionFunction.Parameters[1].LineInfo);

            return false;
        }

        if (lambdaExpressionFunction.Expression is not IBooleanJsonFunction booleanJsonFunction)
        {
            jsonObjectParseError = new JsonObjectParseError(
                $"Lambda expression in function [{selectorName}] is expected to be a boolean expression.",
                lambdaExpressionFunction.Expression.LineInfo);

            return false;
        }

        lambdaPredicate = new PredicateLambdaFunction(
            lambdaExpressionFunction.Parameters[0], booleanJsonFunction);
        return true;
    }

    /// <summary>
    /// Creates an instance of <see cref="IPredicateLambdaFunction"/> from <see cref="ILambdaExpressionFunction"/>. 
    /// </summary>
    /// <param name="selectorName">Selector name.</param>
    /// <param name="lambdaExpressionFunction">Lambda expression function.</param>
    /// <param name="jsonPathLambdaFunction">
    /// Output parameter for generated <see cref="IJsonPathLambdaFunction"/>.
    /// The value is not null if the return value is true.
    /// </param>
    /// <param name="jsonObjectParseError">Error details. The value is not null if the return value is false.</param>
    /// <remarks>
    /// These methods might be modified later to support more general use cases, such as predicates with more than one parameter.
    /// Therefore, use these method with this understanding in mind.
    /// </remarks>
    public static bool TryGetJsonPathLambdaFunctionFromParameter(string selectorName, ILambdaExpressionFunction lambdaExpressionFunction, [NotNullWhen(true)] out IJsonPathLambdaFunction? jsonPathLambdaFunction, [NotNullWhen(false)] out JsonObjectParseError? jsonObjectParseError)
    {
        jsonPathLambdaFunction = null;
        jsonObjectParseError = null;

        if (lambdaExpressionFunction.Parameters.Count != 1)
        {
            jsonObjectParseError = new JsonObjectParseError(
                $"Lambda expression parameter in function [{selectorName}] is expected to have one parameter.",
                lambdaExpressionFunction.Parameters.Count == 0 ? lambdaExpressionFunction.LineInfo : lambdaExpressionFunction.Parameters[1].LineInfo);

            return false;
        }

        if (lambdaExpressionFunction.Expression is not IJsonValuePathJsonFunction jsonValuePathJsonFunction)
        {
            jsonObjectParseError = new JsonObjectParseError(
                $"Lambda expression in function [{selectorName}] is expected to be a json path. Example of valid lambda expression: 'x => x[1].Object1'.",
                lambdaExpressionFunction.Expression.LineInfo);

            return false;
        }

        jsonPathLambdaFunction = new JsonPathLambdaFunction(
            lambdaExpressionFunction.Parameters[0], jsonValuePathJsonFunction);
        return true;
    }
}