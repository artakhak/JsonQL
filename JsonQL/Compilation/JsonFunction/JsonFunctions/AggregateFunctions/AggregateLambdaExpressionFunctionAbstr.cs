using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

public abstract class AggregateLambdaExpressionFunctionAbstr<TAggregationCalculationsData, TResult> : JsonFunctionAbstr, IResolvesVariableValue
    where TResult : struct, IComparable where TAggregationCalculationsData : AggregationCalculationsData<TResult>, new()
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;
    private readonly IUniversalLambdaFunction? _predicateLambdaFunction;
    private readonly IUniversalLambdaFunction? _numericValueLambdaFunction;

    private IUniversalLambdaFunction? _currentLambdaFunctionForVariablevalueEvaluation = null;

    protected AggregateLambdaExpressionFunctionAbstr(
        string functionName,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IUniversalLambdaFunction? predicateLambdaFunction,
        IUniversalLambdaFunction? numericValueLambdaFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonValuePathJsonFunction = jsonValuePathJsonFunction;
        _predicateLambdaFunction = predicateLambdaFunction;
        _numericValueLambdaFunction = numericValueLambdaFunction;
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var result = EvaluatedAggregateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result.Errors.Count > 0)
            return new ParseResult<object?>(result.Errors);

        return new ParseResult<object?>(result.Value);
    }

    private IParseResult<TResult?> EvaluatedAggregateValue(IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var pathEvaluationResult = _jsonValuePathJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (pathEvaluationResult.Errors.Count > 0)
            return new ParseResult<TResult?>(pathEvaluationResult.Errors);

        if (pathEvaluationResult.Value == null)
        {
            return new ParseResult<TResult?>((TResult?)null);
        }

        IReadOnlyList<IParsedValue>? valuesCollection = null;

        if (pathEvaluationResult.Value is ICollectionJsonValuePathLookupResult collectionJsonValuePathLookupResult)
        {
            valuesCollection = collectionJsonValuePathLookupResult.ParsedValues;
        }
        else if (pathEvaluationResult.Value is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult
                 && singleItemJsonValuePathLookupResult.ParsedValue != null)
        {
            if (!singleItemJsonValuePathLookupResult.IsValidPath ||
                singleItemJsonValuePathLookupResult.ParsedValue is not IParsedArrayValue parsedArrayValue)
                return new ParseResult<TResult?>((TResult?)null);

            valuesCollection = parsedArrayValue.Values;
        }

        valuesCollection ??= Array.Empty<IParsedValue>();

        var calculationsData = new TAggregationCalculationsData();
        InitAggregationCalculationsData(calculationsData, valuesCollection);

        List<IParsedValue> filteredCollection = new List<IParsedValue>(valuesCollection.Count);

        var stopEvaluatingValues = false;

        var errors = new List<IJsonObjectParseError>();

        for (var i = 0; i < valuesCollection.Count; ++i)
        {
            var parsedValue = valuesCollection[i];
            var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, i);
            double? lambdaFunctionSelectedValue = null;

            if (_predicateLambdaFunction != null)
            {
                var predicateExpressionResult = GetPredicateLambdaFunctionValue(_predicateLambdaFunction, rootParsedValue, compiledParentRootParsedValues, itemContextData);

                if (predicateExpressionResult.Errors.Count > 0)
                    return new ParseResult<TResult?>(predicateExpressionResult.Errors);

                if (!(predicateExpressionResult.Value ?? false))
                {
                    if (_numericValueLambdaFunction != null)
                    {
                        var lambdaFunctionSelectedValueResult = GetNumericValueLambdaFunctionValue(_numericValueLambdaFunction, rootParsedValue, compiledParentRootParsedValues, itemContextData);

                        if (lambdaFunctionSelectedValueResult.Errors.Count > 0)
                            return new ParseResult<TResult?>(lambdaFunctionSelectedValueResult.Errors);

                        if (lambdaFunctionSelectedValueResult.Value == null)
                            continue;

                        lambdaFunctionSelectedValue = lambdaFunctionSelectedValueResult.Value;
                    }

                    UpdateAggregatedValue(calculationsData, itemContextData, lambdaFunctionSelectedValue, false, errors, ref stopEvaluatingValues);
                    continue;
                }
            }

            filteredCollection.Add(parsedValue);

            if (_numericValueLambdaFunction != null)
            {
                var lambdaFunctionSelectedValueResult = GetNumericValueLambdaFunctionValue(_numericValueLambdaFunction, rootParsedValue, compiledParentRootParsedValues, itemContextData);

                if (lambdaFunctionSelectedValueResult.Errors.Count > 0)
                    return new ParseResult<TResult?>(lambdaFunctionSelectedValueResult.Errors);

                if (lambdaFunctionSelectedValueResult.Value == null)
                    continue;

                lambdaFunctionSelectedValue = lambdaFunctionSelectedValueResult.Value;
            }

            UpdateAggregatedValue(calculationsData, itemContextData, lambdaFunctionSelectedValue, true, errors, ref stopEvaluatingValues);

            if (errors.Count > 0)
                return new ParseResult<TResult?>(errors);

            if (stopEvaluatingValues)
                break;
        }

        if (filteredCollection.Count == 0)
            return GetValueForEmptyCollection(rootParsedValue, compiledParentRootParsedValues, contextData);

        UpdateAggregatedValueOnAllItemsEvaluated(filteredCollection, calculationsData, errors);

        if (errors.Count > 0)
            return new ParseResult<TResult?>(errors);

        return new ParseResult<TResult?>(calculationsData.GetResult());
    }

    private IParseResult<bool?> GetPredicateLambdaFunctionValue(IUniversalLambdaFunction predicateLambdaFunction, IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData contextData)
    {
        try
        {
            _currentLambdaFunctionForVariablevalueEvaluation = _predicateLambdaFunction;
            if (predicateLambdaFunction.LambdaExpressionFunction is IBooleanJsonFunction booleanJsonFunction)
                return booleanJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

            var predicatedParseResult = predicateLambdaFunction.LambdaExpressionFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

            if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(predicatedParseResult.Value, TypeCode.Boolean, out var jsonComparable) ||
                jsonComparable.Value is not bool boolValue)
                return new ParseResult<bool?>(false);

            return new ParseResult<bool?>(boolValue);
        }
        finally
        {
            _currentLambdaFunctionForVariablevalueEvaluation = null;
        }
    }

    private IParseResult<double?> GetNumericValueLambdaFunctionValue(IUniversalLambdaFunction numericValueLambdaFunction, IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData contextData)
    {
        try
        {
            _currentLambdaFunctionForVariablevalueEvaluation = _numericValueLambdaFunction;

            if (numericValueLambdaFunction.LambdaExpressionFunction is IDoubleJsonFunction doubleJsonFunction)
                return doubleJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

            var numericValueParseResult = numericValueLambdaFunction.LambdaExpressionFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

            if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(numericValueParseResult.Value, TypeCode.Double, out var jsonComparable) ||
                jsonComparable.Value is not double doubleValue)
                return new ParseResult<double?>((double?)null);

            return new ParseResult<double?>(doubleValue);
        }
        finally
        {
            _currentLambdaFunctionForVariablevalueEvaluation = null;
        }

    }

    protected virtual void InitAggregationCalculationsData(TAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
    }

    protected virtual IParseResult<TResult?> GetValueForEmptyCollection(IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<TResult?>((TResult?) null);
    }

    protected abstract void UpdateAggregatedValue(TAggregationCalculationsData calculationsData,
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue,
        bool predicateEvaluationResult,
        List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues);

    protected virtual void UpdateAggregatedValueOnAllItemsEvaluated(IReadOnlyList<IParsedValue> filteredEvaluatedValues,
        TAggregationCalculationsData calculationsData, List<IJsonObjectParseError> errors)
    {

    }

    /// <inheritdoc />
    public IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData)
    {
        return LambdaFunctionParameterResolverHelpers.TryEvaluateLambdaFunctionParameterValue(_currentLambdaFunctionForVariablevalueEvaluation, variableName, contextData);
    }
}
