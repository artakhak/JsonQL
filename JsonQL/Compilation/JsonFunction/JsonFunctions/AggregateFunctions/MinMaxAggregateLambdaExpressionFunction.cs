using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "Min(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(Max(Object1.Array1, x => x % 2 == 0))==156"
/// </summary>
public class MinMaxAggregateLambdaExpressionFunction : AggregateLambdaExpressionFunctionAbstr<MinMaxAggregationCalculationsData, double>, IDoubleJsonFunction
{
    private readonly bool _isMinAggregation;

    public MinMaxAggregateLambdaExpressionFunction(
        string functionName,
        bool isMinAggregation,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction? lambdaPredicate,
        IUniversalLambdaFunction? numericValueLambdaFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName,
            jsonValuePathJsonFunction, lambdaPredicate, numericValueLambdaFunction, jsonFunctionContext, lineInfo)
    {
        _isMinAggregation = isMinAggregation;
    }

    /// <inheritdoc />
    public IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(LineInfo);
    }

    protected override void InitAggregationCalculationsData(MinMaxAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
        base.InitAggregationCalculationsData(calculationsData, valuesCollection);
        calculationsData.MinMaxValue = _isMinAggregation ? Double.MaxValue : Double.MinValue;
    }

    protected override void UpdateAggregatedValue(MinMaxAggregationCalculationsData calculationsData, 
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue, bool predicateEvaluationResult, List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues)
    {
        if (!predicateEvaluationResult)
            return;

        double doubleValue;

        if (lambdaFunctionSelectedValue != null)
        {
            doubleValue = lambdaFunctionSelectedValue.Value;
        }
        else
        {
            if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(contextData?.EvaluatedValue, TypeCode.Double, out var jsonComparable))
                return;

            doubleValue = (double)jsonComparable.Value;
        }

        if (_isMinAggregation)
        {
            if (doubleValue < calculationsData.MinMaxValue)
                calculationsData.MinMaxValue = doubleValue;
            return;
        }

        if (doubleValue > calculationsData.MinMaxValue)
            calculationsData.MinMaxValue = doubleValue;
    }
}