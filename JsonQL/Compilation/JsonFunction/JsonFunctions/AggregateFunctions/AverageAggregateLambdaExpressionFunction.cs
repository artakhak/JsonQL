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
    public IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
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