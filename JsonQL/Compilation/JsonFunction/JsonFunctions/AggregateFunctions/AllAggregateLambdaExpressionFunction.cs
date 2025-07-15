// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "All(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(All(Object1.Array1, x => typeof(x)=='number'))==6"
/// </summary>
public class AllAggregateLambdaExpressionFunction : AggregateLambdaExpressionFunctionAbstr<AllAggregationCalculationsData, bool>, IBooleanJsonFunction
{
    /// <summary>
    /// Represents an aggregate lambda expression function implementation that evaluates
    /// whether all elements in a specified collection satisfy a given predicate.
    /// </summary>
    /// <remarks>
    /// This class processes a collection and determines if all elements fulfill a specified condition,
    /// utilizing a provided predicate lambda function. It is a specialized aggregate function suitable
    /// for JSON data handling contexts.
    /// </remarks>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="jsonValuePathJsonFunction">The JSON value path function to be evaluated against the input.</param>
    /// <param name="predicateLambdaFunction">The lambda function defining the predicate to apply on elements of the collection.</param>
    /// <param name="jsonFunctionContext">The execution context for the JSON function, which holds the environmental data needed during evaluation.</param>
    /// <param name="lineInfo">Optional line information for error diagnostics, referencing the relevant position in the source.</param>
    public AllAggregateLambdaExpressionFunction(string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction predicateLambdaFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonValuePathJsonFunction, predicateLambdaFunction, null, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToBoolean(this.LineInfo);
    }
  
    /// <inheritdoc />
    protected override void InitAggregationCalculationsData(AllAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
        base.InitAggregationCalculationsData(calculationsData, valuesCollection);
        calculationsData.Result = true;
    }

    /// <inheritdoc />
    protected override void UpdateAggregatedValue(
        AllAggregationCalculationsData calculationsData, 
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue,
        bool predicateEvaluationResult,
        List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues)
    {
        if (predicateEvaluationResult)
            return;

        calculationsData.Result = false;
        stopEvaluatingValues = true;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetValueForEmptyCollection(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<bool?>(true);
    }
}