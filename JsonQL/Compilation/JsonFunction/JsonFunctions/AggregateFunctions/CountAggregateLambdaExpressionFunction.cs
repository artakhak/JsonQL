// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "$Count(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(Count(Object1.Array1, x => typeof(x)=='number'))==6"
/// </summary>
public class CountAggregateLambdaExpressionFunction: AggregateLambdaExpressionFunctionAbstr<CountAggregationCalculationsData, double>, IDoubleJsonFunction
{
    private readonly IUniversalLambdaFunction? _lambdaPredicate;

    /// <summary>
    /// Represents a function for counting elements within a JSON structure using a lambda expression predicate.
    /// Inherits from <see cref="AggregateLambdaExpressionFunctionAbstr{CountAggregationCalculationsData, double}"/>.
    /// </summary>
    /// <remarks>
    /// This function aggregates values based on a lambda expression and counts the qualifying elements.
    /// </remarks>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="jsonValuePathJsonFunction">The JSON value path function used to provide the data to process.</param>
    /// <param name="lambdaPredicate">The lambda expression defining the filtering or evaluation logic for the count operation.</param>
    /// <param name="jsonFunctionContext">The context for evaluating JSON function values.</param>
    /// <param name="lineInfo">Optional line information for the function.</param>
    public CountAggregateLambdaExpressionFunction(string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction? lambdaPredicate,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : 
        base(functionName, jsonValuePathJsonFunction, lambdaPredicate, null, jsonFunctionContext, lineInfo)
    {
        _lambdaPredicate = lambdaPredicate;
    }

    /// <inheritdoc />
    public IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(LineInfo);
    }

    /// <inheritdoc />
    protected override void InitAggregationCalculationsData(CountAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
        base.InitAggregationCalculationsData(calculationsData, valuesCollection);

        if (_lambdaPredicate == null)
        {
            calculationsData.NumberOfEvaluatedValues = valuesCollection.Count;
        }
    }

    /// <inheritdoc />
    protected override void UpdateAggregatedValue(CountAggregationCalculationsData calculationsData, 
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue, bool predicateEvaluationResult, List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues)
    {
        if (this._lambdaPredicate == null)
        {
            stopEvaluatingValues = true;
            return;
        }

        if (!predicateEvaluationResult)
            return;
            
        ++calculationsData.NumberOfEvaluatedValues;
    }

    /// <inheritdoc />
    protected override IParseResult<double?> GetValueForEmptyCollection(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<double?>(0);
    }
}