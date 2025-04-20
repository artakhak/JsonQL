using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "All(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(All(Object1.Array1, x => typeof(x)=='number'))==6"
/// </summary>
public class AllAggregateLambdaExpressionFunction : AggregateLambdaExpressionFunctionAbstr<AllAggregationCalculationsData, bool>, IBooleanJsonFunction
{
    public AllAggregateLambdaExpressionFunction(string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction predicateLambdaFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonValuePathJsonFunction, predicateLambdaFunction, null, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<bool?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
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