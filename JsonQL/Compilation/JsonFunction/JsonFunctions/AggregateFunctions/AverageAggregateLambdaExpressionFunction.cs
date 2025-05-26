// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "Average(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(Average(Object1.Array1, x => x % 2 == 0))==156"
/// </summary>
public class AverageAggregateLambdaExpressionFunction : AggregateLambdaExpressionFunctionAbstr<AverageValueAggregationCalculationsData, double>, IDoubleJsonFunction
{
    /// <summary>
    /// Represents an aggregate lambda expression function that calculates the average value
    /// based on specified criteria and numeric expressions within a JSON context.
    /// </summary>
    /// <param name="functionName">The name of the aggregate function.</param>
    /// <param name="jsonValuePathJsonFunction">The JSON value path function to extract values for the calculation.</param>
    /// <param name="lambdaPredicate">An optional predicate lambda function to filter the data.</param>
    /// <param name="numericValueLambdaFunction">An optional lambda function to compute numeric values from the data.</param>
    /// <param name="jsonFunctionContext">The context to evaluate JSON function values.</param>
    /// <param name="lineInfo">Optional line information for debugging and error context.</param>
    public AverageAggregateLambdaExpressionFunction(string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction? lambdaPredicate,
        IUniversalLambdaFunction? numericValueLambdaFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName,
            jsonValuePathJsonFunction, lambdaPredicate, numericValueLambdaFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(LineInfo);
    }

    /// <inheritdoc />
    protected override void InitAggregationCalculationsData(AverageValueAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
        base.InitAggregationCalculationsData(calculationsData, valuesCollection);
        calculationsData.CurrentValue = 0;
        calculationsData.NumberOfEvaluatedValues = 0;
    }

    /// <inheritdoc />
    protected override void UpdateAggregatedValue(AverageValueAggregationCalculationsData calculationsData, 
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue, 
        bool predicateEvaluationResult, List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues)
    {
        if (!predicateEvaluationResult)
            return;

        if (lambdaFunctionSelectedValue != null)
        {
            calculationsData.CurrentValue += lambdaFunctionSelectedValue.Value;
        }
        else 
        {
            if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(contextData?.EvaluatedValue, TypeCode.Double, out var jsonComparable))
                return;

            calculationsData.CurrentValue += (double)jsonComparable.Value;
        }
        
        ++calculationsData.NumberOfEvaluatedValues;
    }
}