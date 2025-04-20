using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Evaluating expressions like:<br/>
/// "$Count(Object1.Array1, x => x > 10 || any(x.Capitalization > 300) || x.count() >= 2)"<br/>
/// "$(Count(Object1.Array1, x => typeof(x)=='number'))==6"
/// </summary>
public class CountAggregateLambdaExpressionFunction: AggregateLambdaExpressionFunctionAbstr<CountAggregationCalculationsData, double>, IDoubleJsonFunction
{
    private readonly IUniversalLambdaFunction? _lambdaPredicate;

    public CountAggregateLambdaExpressionFunction(string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction? lambdaPredicate, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : 
        base(functionName, jsonValuePathJsonFunction, lambdaPredicate, null, jsonFunctionContext, lineInfo)
    {
        _lambdaPredicate = lambdaPredicate;
    }

    /// <inheritdoc />
    public IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(LineInfo);
    }

    protected override void InitAggregationCalculationsData(CountAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
        base.InitAggregationCalculationsData(calculationsData, valuesCollection);

        if (_lambdaPredicate == null)
        {
            calculationsData.NumberOfEvaluatedValues = valuesCollection.Count;
        }
    }

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