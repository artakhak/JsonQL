// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Helper methods that can be used in implementations of <see cref="IJsonValueCollectionItemsSelectorPathElementFactory"/>
/// </summary>
public interface IJsonValueLookupHelpers
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
    bool TryGetLambdaPredicateFromParameter(string selectorName, ILambdaExpressionFunction lambdaExpressionFunction, [NotNullWhen(true)] out IPredicateLambdaFunction? lambdaPredicate, [NotNullWhen(false)] out JsonObjectParseError? jsonObjectParseError);

    /// <summary>
    /// Creates an instance of <see cref="IPredicateLambdaFunction"/> from <see cref="ILambdaExpressionFunction"/>. 
    /// </summary>
    /// <param name="selectorName">Selector name.</param>
    /// <param name="lambdaExpressionFunction">Lambda expression function.</param>
    /// <param name="jsonPathLambdaFunction">
    /// Output parameter for generated <see cref="ISelectCollectionItemsPathElementLambdaFunction"/>.
    /// The value is not null if the return value is true.
    /// </param>
    /// <param name="jsonObjectParseError">Error details. The value is not null if the return value is false.</param>
    /// <param name="jsonFunctionContext">Json function context</param>
    /// <param name="lineInfo">Line info.</param>
    /// <remarks>
    /// These methods might be modified later to support more general use cases, such as predicates with more than one parameter.
    /// Therefore, use these method with this understanding in mind.
    /// </remarks>
    bool TryGetJsonPathLambdaFunctionFromParameter(string selectorName, ILambdaExpressionFunction lambdaExpressionFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo,
        [NotNullWhen(true)] out ISelectCollectionItemsPathElementLambdaFunction? jsonPathLambdaFunction, [NotNullWhen(false)] out JsonObjectParseError? jsonObjectParseError);
}

/// <inheritdoc />
public class JsonValueLookupHelpers: IJsonValueLookupHelpers
{
    private readonly IEvaluatesJsonValuePathLookupResultFactory _evaluatesJsonValuePathLookupResultFactory;

    public JsonValueLookupHelpers(IEvaluatesJsonValuePathLookupResultFactory evaluatesJsonValuePathLookupResultFactory)
    {
        _evaluatesJsonValuePathLookupResultFactory = evaluatesJsonValuePathLookupResultFactory;
    }

    /// <inheritdoc />
    public bool TryGetLambdaPredicateFromParameter(string selectorName, ILambdaExpressionFunction lambdaExpressionFunction,
        [NotNullWhen(true)] out IPredicateLambdaFunction? lambdaPredicate, [NotNullWhen(false)] out JsonObjectParseError? jsonObjectParseError)
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

    /// <inheritdoc />
    public bool TryGetJsonPathLambdaFunctionFromParameter(string selectorName, ILambdaExpressionFunction lambdaExpressionFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo,
        [NotNullWhen(true)] out ISelectCollectionItemsPathElementLambdaFunction? jsonPathLambdaFunction, [NotNullWhen(false)] out JsonObjectParseError? jsonObjectParseError)
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
        
        if (lambdaExpressionFunction.Expression is not IEvaluatesJsonValuePathLookupResult evaluatesJsonValuePathLookupResult)
        {
            var evaluatesJsonValuePathLookupResultGenerated = _evaluatesJsonValuePathLookupResultFactory.TryCreate(lambdaExpressionFunction.Expression, jsonFunctionContext, lineInfo);

            if (evaluatesJsonValuePathLookupResultGenerated == null)
            {
                jsonObjectParseError = new JsonObjectParseError(
                    $"Lambda expression in function [{selectorName}] is expected to be a json path or an expression that can be converted to json object. Examples of valid lambda expression are: 'x => x[1].Object1', 'x => 0.1 * employee.Salary', x => Concat('$', x.Salary).",
                    lambdaExpressionFunction.Expression.LineInfo);

                return false;
            }

            evaluatesJsonValuePathLookupResult = evaluatesJsonValuePathLookupResultGenerated;
        }

        jsonPathLambdaFunction = new SelectCollectionItemsPathElementLambdaFunction(
            lambdaExpressionFunction.Parameters[0], evaluatesJsonValuePathLookupResult);
        return true;
    }
}
