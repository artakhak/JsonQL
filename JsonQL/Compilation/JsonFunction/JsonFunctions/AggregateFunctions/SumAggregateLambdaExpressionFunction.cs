// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "Sum(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(Sum(Object1.Array1, x => x % 2 == 0))==156"
/// </summary>
public class SumAggregateLambdaExpressionFunction : AggregateLambdaExpressionFunctionAbstr<SumAggregationCalculationsData, double>, IDoubleJsonFunction
{
    /// <summary>
    /// Represents a sum aggregate function that operates on JSON data with support for lambda expressions.
    /// </summary>
    /// <remarks>
    /// This class is a specialized implementation of <see cref="AggregateLambdaExpressionFunctionAbstr{TAggregationCalculationsData, TResult}"/>
    /// designed to compute the sum of numeric values within a collection of JSON elements, optionally filtered by a predicate.
    /// </remarks>
    /// <param name="functionName">The name of the aggregate function.</param>
    /// <param name="jsonValuePathJsonFunction">Specifies the JSON path function to extract values from JSON data.</param>
    /// <param name="lambdaPredicate">Optional lambda predicate used to filter the JSON elements.</param>
    /// <param name="numericValueLambdaFunction">Optional lambda function to calculate numeric values from JSON elements.</param>
    /// <param name="jsonFunctionContext">The context for evaluating JSON function values.</param>
    /// <param name="lineInfo">The line information for debugging or error reporting.</param>
    public SumAggregateLambdaExpressionFunction(
        string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction? lambdaPredicate,
        IUniversalLambdaFunction? numericValueLambdaFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonValuePathJsonFunction, lambdaPredicate, numericValueLambdaFunction,  jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(LineInfo);
    }

    /// <inheritdoc />
    protected override void UpdateAggregatedValue(SumAggregationCalculationsData calculationsData, 
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue, 
        bool predicateEvaluationResult, List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues)
    {
        if (!predicateEvaluationResult)
            return;

        if (lambdaFunctionSelectedValue != null)
        {
            calculationsData.Sum += lambdaFunctionSelectedValue.Value;
        }
        else
        {
            if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(contextData?.EvaluatedValue, TypeCode.Double, out var jsonComparable))
                return;

            calculationsData.Sum += (double)jsonComparable.Value;
        }
    }
}