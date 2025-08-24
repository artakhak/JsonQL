// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a factory interface for creating instances of <see cref="IEvaluatesJsonValuePathLookupResult"/>.
/// The factory is responsible for determining the appropriate implementation of the
/// JSON value path lookup evaluation results, based on the provided JSON function, evaluation context, and optional line information.
/// </summary>
public interface IEvaluatesJsonValuePathLookupResultFactory
{
    /// <summary>
    /// Attempts to create an instance of <see cref="IEvaluatesJsonValuePathLookupResult"/>
    /// based on the provided JSON function, evaluation context, and optional line information.
    /// </summary>
    /// <param name="jsonFunction">The JSON function to evaluate and create the lookup result for.</param>
    /// <param name="jsonFunctionContext">The evaluation context used during the creation process.</param>
    /// <param name="lineInfo">Optional line information related to the evaluation context.</param>
    /// <returns>
    /// An instance of <see cref="IEvaluatesJsonValuePathLookupResult"/> if creation is successful;
    /// otherwise, null if the creation could not be performed.
    /// </returns>
    IEvaluatesJsonValuePathLookupResult? TryCreate(IJsonFunction jsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo);
}

/// <inheritdoc />
public class EvaluatesJsonValuePathLookupResultFactory : IEvaluatesJsonValuePathLookupResultFactory
{
    private readonly IStringFormatter _stringFormatter;

    public EvaluatesJsonValuePathLookupResultFactory(IStringFormatter stringFormatter)
    {
        _stringFormatter = stringFormatter;
    }

    /// <inheritdoc />
    public IEvaluatesJsonValuePathLookupResult? TryCreate(IJsonFunction jsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        switch (jsonFunction)
        {
            case IDoubleJsonFunction doubleJsonFunction:
                return new EvaluatesJsonValuePathLookupResultFromDoubleValue(_stringFormatter, JsonFunctionNames.ExpressionConvertedToParsedSimpleValue, doubleJsonFunction, jsonFunctionContext, lineInfo);

            case IStringJsonFunction stringJsonFunction:
                return new EvaluatesJsonValuePathLookupResultFromStringValue(_stringFormatter, JsonFunctionNames.ExpressionConvertedToParsedSimpleValue, stringJsonFunction, jsonFunctionContext, lineInfo);

            case IDateTimeJsonFunction dateTimeJsonFunction:
                return new EvaluatesJsonValuePathLookupResultFromDateTimeValue(_stringFormatter, JsonFunctionNames.ExpressionConvertedToParsedSimpleValue, dateTimeJsonFunction, jsonFunctionContext, lineInfo);

            case IBooleanJsonFunction booleanJsonFunction:
                return new EvaluatesJsonValuePathLookupResultFromBooleanValue(_stringFormatter, JsonFunctionNames.ExpressionConvertedToParsedSimpleValue, booleanJsonFunction, jsonFunctionContext, lineInfo);

            default:
                return null;
        }
    }
}