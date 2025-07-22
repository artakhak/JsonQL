// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "Any(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(Any(Object1.Array1, x => typeof(x)=='number'))==6"
/// </summary>
public class AnyAggregateLambdaExpressionFunction : AggregateLambdaExpressionFunctionAbstr<AnyAggregationCalculationsData, bool>, IBooleanJsonFunction
{
    /// <summary>
    /// Represents a specific aggregate lambda expression function that determines if any element in a collection satisfies a given condition.
    /// </summary>
    /// <remarks>
    /// This function evaluates a boolean predicate over each element of a collection, returning true if at least one element matches the predicate.
    /// It extends the <see cref="AggregateLambdaExpressionFunctionAbstr{TAggregationCalculationsData, TResult}"/> class with specific functionality for "Any" aggregation.
    /// </remarks>
    /// <param name="functionName">The name of the function being evaluated.</param>
    /// <param name="jsonValuePathJsonFunction">The JSON Value Path function providing the source collection to evaluate.</param>
    /// <param name="lambdaPredicate">The lambda function applied as a predicate to each element in the source collection.</param>
    /// <param name="jsonFunctionContext">The context in which function evaluation occurs, providing necessary evaluation scope and resolution.</param>
    /// <param name="lineInfo">Optional line information for error tracking and debugging in the source JSON.</param>
    public AnyAggregateLambdaExpressionFunction(string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction? lambdaPredicate,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName,
            jsonValuePathJsonFunction, lambdaPredicate, null, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToBoolean(LineInfo);
    }

    /// <inheritdoc />
    protected override void InitAggregationCalculationsData(AnyAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
        base.InitAggregationCalculationsData(calculationsData, valuesCollection);

        calculationsData.Result = false;
    }

    /// <inheritdoc />
    protected override void UpdateAggregatedValue(AnyAggregationCalculationsData calculationsData, 
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue,
         bool predicateEvaluationResult,
        List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues)
    {
        if (!predicateEvaluationResult)
            return;

        calculationsData.Result = true;
        stopEvaluatingValues = true;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetValueForEmptyCollection(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<bool?>(false);
    }
}
