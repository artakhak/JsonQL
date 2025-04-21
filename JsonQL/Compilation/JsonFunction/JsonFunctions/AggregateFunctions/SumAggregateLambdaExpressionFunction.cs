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
    public IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
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