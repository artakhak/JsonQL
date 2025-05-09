// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

/// <summary>
/// Represents an abstract base class for aggregate JSON functions that utilize lambda expressions.
/// This class derives from <see cref="JsonFunctionAbstr"/> and implements <see cref="IResolvesVariableValue"/>.
/// It is designed to handle aggregate operations with specified calculation data and result types.
/// </summary>
/// <typeparam name="TAggregationCalculationsData">
/// The type of aggregation calculations data utilized by the function.
/// It must derive from <see cref="AggregationCalculationsData{TResult}"/> and have a parameterless constructor.
/// </typeparam>
/// <typeparam name="TResult">
/// The type of result produced by the function. It must be a struct and implement <see cref="IComparable"/>.
/// </typeparam>
public abstract class AggregateLambdaExpressionFunctionAbstr<TAggregationCalculationsData, TResult> : JsonFunctionAbstr, IResolvesVariableValue
    where TResult : struct, IComparable where TAggregationCalculationsData : AggregationCalculationsData<TResult>, new()
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;
    private readonly IUniversalLambdaFunction? _predicateLambdaFunction;
    private readonly IUniversalLambdaFunction? _numericValueLambdaFunction;

    private IUniversalLambdaFunction? _currentLambdaFunctionForVariableValueEvaluation;

    /// <summary>
    /// Represents an abstract base class for aggregate lambda expression functions,
    /// extending the core functionality of JSON functions to support aggregation logic
    /// via customizable predicates and numeric value lambdas.
    /// </summary>
    /// <typeparam name="TAggregationCalculationsData">
    /// The type of aggregation calculation data to process, which must be a derived type
    /// of AggregationCalculationsData and provide a parameterless constructor.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The result type for aggregation calculations. This must be a value type that
    /// implements IComparable.
    /// </typeparam>
    /// <param name="functionName">
    /// The name of the JSON function being implemented.
    /// </param>
    /// <param name="jsonValuePathJsonFunction">
    /// An object that provides JSON value path resolution functionality.
    /// </param>
    /// <param name="predicateLambdaFunction">
    /// An optional lambda function defining a predicate for filtering JSON data during
    /// aggregation.
    /// </param>
    /// <param name="numericValueLambdaFunction">
    /// An optional lambda function for extracting numeric values from JSON data to be
    /// used in aggregation calculations.
    /// </param>
    /// <param name="jsonFunctionContext">
    /// The context for evaluating values during the execution of the JSON function.
    /// </param>
    /// <param name="lineInfo">
    /// An optional parameter for providing line information relevant to JSON function
    /// evaluation, aiding in error reporting and diagnostics.
    /// </param>
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

        var filteredCollection = new List<IParsedValue>(valuesCollection.Count);

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
            _currentLambdaFunctionForVariableValueEvaluation = _predicateLambdaFunction;
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
            _currentLambdaFunctionForVariableValueEvaluation = null;
        }
    }

    private IParseResult<double?> GetNumericValueLambdaFunctionValue(IUniversalLambdaFunction numericValueLambdaFunction, IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData contextData)
    {
        try
        {
            _currentLambdaFunctionForVariableValueEvaluation = _numericValueLambdaFunction;

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
            _currentLambdaFunctionForVariableValueEvaluation = null;
        }

    }

    protected virtual void InitAggregationCalculationsData(TAggregationCalculationsData calculationsData, IReadOnlyList<IParsedValue> valuesCollection)
    {
    }

    /// <summary>
    /// Retrieves a parsed result representing the value to be used for an empty collection within
    /// the context of an aggregate lambda expression function.
    /// </summary>
    /// <param name="rootParsedValue">
    /// The root parsed value that serves as the base for the aggregation evaluation.
    /// </param>
    /// <param name="compiledParentRootParsedValues">
    /// A read-only list of compiled root parsed values associated with parent elements for
    /// facilitating the aggregation evaluation.
    /// </param>
    /// <param name="contextData">
    /// An optional parameter providing additional context data required during the evaluation
    /// of the aggregation.
    /// </param>
    /// <returns>
    /// A parsed result containing the default value for the aggregate function when applied to
    /// an empty collection.
    /// </returns>
    protected virtual IParseResult<TResult?> GetValueForEmptyCollection(IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<TResult?>((TResult?) null);
    }

    /// <summary>
    /// Updates the aggregated value in the calculation data based on the provided parameters,
    /// including the selected value, predicate evaluation result, and associated errors.
    /// This method is integral to performing aggregation logic within derived aggregate
    /// JSON functions.
    /// </summary>
    /// <param name="calculationsData">
    /// The aggregation calculations data object to be updated, containing the current state
    /// of the aggregation process and results.
    /// </param>
    /// <param name="contextData">
    /// Optional context data for the JSON function evaluation, which may provide additional
    /// information for processing or evaluation.
    /// </param>
    /// <param name="lambdaFunctionSelectedValue">
    /// The numeric value selected by the lambda function, used for aggregation calculations.
    /// Can be null if no value was selected or the lambda function's result was not numeric.
    /// </param>
    /// <param name="predicateEvaluationResult">
    /// The result of evaluating a predicate against the current JSON data item,
    /// indicating whether the item satisfies specified conditions.
    /// </param>
    /// <param name="errors">
    /// A list to which any JSON object parse errors encountered during processing may be
    /// added, aiding in error tracking and diagnostics.
    /// </param>
    /// <param name="stopEvaluatingValues">
    /// A reference to a boolean flag that can be set to true to interrupt further
    /// evaluation of JSON data items, typically under certain aggregation conditions.
    /// </param>
    protected abstract void UpdateAggregatedValue(TAggregationCalculationsData calculationsData,
        IJsonFunctionEvaluationContextData? contextData, double? lambdaFunctionSelectedValue,
        bool predicateEvaluationResult,
        List<IJsonObjectParseError> errors, ref bool stopEvaluatingValues);

    /// <summary>
    /// Updates the aggregated value once all items have been evaluated, performing necessary
    /// calculations based on the filtered values and managing any errors encountered during the process.
    /// </summary>
    /// <param name="filteredEvaluatedValues">
    /// A read-only list of parsed values that have been filtered and are to be used
    /// for aggregation calculations.
    /// </param>
    /// <param name="calculationsData">
    /// An instance of aggregation calculation data that stores intermediate and final results
    /// of the aggregation process.
    /// </param>
    /// <param name="errors">
    /// A list that collects any parsing or evaluation errors that occurred during the aggregation process.
    /// </param>
    protected virtual void UpdateAggregatedValueOnAllItemsEvaluated(IReadOnlyList<IParsedValue> filteredEvaluatedValues,
        TAggregationCalculationsData calculationsData, List<IJsonObjectParseError> errors)
    {

    }

    /// <inheritdoc />
    public IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData)
    {
        return LambdaFunctionParameterResolverHelpers.TryEvaluateLambdaFunctionParameterValue(_currentLambdaFunctionForVariableValueEvaluation, variableName, contextData);
    }
}
