// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Provides a factory for creating instances of <see cref="IJsonValueCollectionItemsSelectorPathElement"/>
/// based on specified parameters like parsed values, function context, and function name.
/// </summary>
public interface IJsonValueCollectionItemsSelectorPathElementFactory
{
    // TODO: See if it makes sense to move this method somewhere else.
    // Currently, it makes sense to have this in the same interface since the implementation
    // of <see cref="Create"/> needs access to collection function names list too, but this method will be re-evaluated.
    /// <summary>
    /// Determines whether the specified function name corresponds to a JSON value collection items selector function.
    /// </summary>
    /// <param name="functionName">The name of the function to check.</param>
    /// <returns><c>true</c> if the function name is recognized as a JSON value collection items selector function; otherwise, <c>false</c>.</returns>
    bool IsJsonValueCollectionItemsSelectorFunction(string functionName);

    /// <summary>
    /// Creates an instance of <see cref="IJsonValueCollectionItemsSelectorPathElement"/> based on the provided parameters.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value to be used for constructing the selector path element.</param>
    /// <param name="bracesExpressionItem">The braces expression item associated with the selector path.</param>
    /// <param name="functionName">The name of the function associated with the selector.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function during compilation.</param>
    /// <param name="lineInfo">Optional line information for the parsed content.</param>
    /// <returns>A parse result that includes the created <see cref="IJsonValueCollectionItemsSelectorPathElement"/>.</returns>
    IParseResult<IJsonValueCollectionItemsSelectorPathElement> Create(IParsedSimpleValue parsedSimpleValue,
        IBracesExpressionItem bracesExpressionItem, string functionName,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo);
}

/// <inheritdoc />
public class JsonValueCollectionItemsSelectorPathElementFactory : IJsonValueCollectionItemsSelectorPathElementFactory
{
    private IJsonFunctionFromExpressionParser? _jsonFunctionFromExpressionParser;

    private static readonly HashSet<string> _collectionItemSelectorFunctionNames = new HashSet<string>(StringComparer.Ordinal)
    {
        {JsonValuePathFunctionNames.FlattenCollectionItemsSelectorFunction},
        {JsonValuePathFunctionNames.ReverseCollectionItemsSelectorFunction},
        {JsonValuePathFunctionNames.WhereCollectionItemsFunction},
        {JsonValuePathFunctionNames.SelectCollectionItemsFunction},
        {JsonValuePathFunctionNames.FirstCollectionItemSelectorFunction},
        {JsonValuePathFunctionNames.LastCollectionItemSelectorFunction},
        {JsonValuePathFunctionNames.CollectionItemSelectorFunction}
    };

    /// <summary>
    /// This value cannot be injected in the constructor because of circular dependencies.
    /// The value is not in interface <see cref="IJsonValuePathJsonFunctionParser"/> and should be set in DI setup.
    /// </summary>
    public IJsonFunctionFromExpressionParser JsonFunctionFromExpressionParser
    {
        get => _jsonFunctionFromExpressionParser ?? throw new NullReferenceException($"The value of [{nameof(JsonFunctionFromExpressionParser)}] was not set.");
        set
        {
            if (_jsonFunctionFromExpressionParser != null)
                throw new ApplicationException($"The value of [{nameof(JsonFunctionFromExpressionParser)}] can be set only once.");

            _jsonFunctionFromExpressionParser = value;
        }
    }

    /// <inheritdoc />
    public bool IsJsonValueCollectionItemsSelectorFunction(string functionName)
    {
        return _collectionItemSelectorFunctionNames.Contains(functionName);
    }

    /// <inheritdoc />
    public IParseResult<IJsonValueCollectionItemsSelectorPathElement> Create(IParsedSimpleValue parsedSimpleValue,
        IBracesExpressionItem bracesExpressionItem, string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        var functionNameLiteralExpression = bracesExpressionItem.NameLiteral;

        if (functionNameLiteralExpression == null)
        {
            // This will never happen, however we still should do a null check.
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(
                new JsonObjectParseError("Function name is missing",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem))
            ));
        }

        var functionParameters = bracesExpressionItem.Parameters;

        switch (functionName)
        {
            case JsonValuePathFunctionNames.FlattenCollectionItemsSelectorFunction:
                return CreateFlattenCollectionItemsPathElement(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);

            case JsonValuePathFunctionNames.ReverseCollectionItemsSelectorFunction:
                return CreateReverseCollectionItemsPathElement(parsedSimpleValue, functionName, functionParameters, lineInfo);

            case JsonValuePathFunctionNames.WhereCollectionItemsFunction:
                return CreateWhereCollectionItemsPathElement(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);

            case JsonValuePathFunctionNames.SelectCollectionItemsFunction:
                return CreateSelectCollectionItemsPathElement(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);

            case JsonValuePathFunctionNames.FirstCollectionItemSelectorFunction:
                return CreateSelectFirstLastCollectionItemsPathElement(true, parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);

            case JsonValuePathFunctionNames.LastCollectionItemSelectorFunction:
                return CreateSelectFirstLastCollectionItemsPathElement(false, parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);

            case JsonValuePathFunctionNames.CollectionItemSelectorFunction:
                return CreateSelectCollectionItemPathElement(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(
            new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                lineInfo)
        ));
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateFlattenCollectionItemsPathElement(
        IParsedSimpleValue parsedSimpleValue, string functionName, IReadOnlyList<IExpressionItemBase> functionParameters, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<ILambdaExpressionFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("criteria", typeof(ILambdaExpressionFunction), false),
            parametersJsonFunctionContext, lineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(parametersParseResult.Errors);

        IPredicateLambdaFunction? lambdaPredicate = null;

        if (parametersParseResult.Value != null)
        {
            if (!JsonValueLookupHelpers.TryGetLambdaPredicateFromParameter(functionName, parametersParseResult.Value, out lambdaPredicate,
                    out var jsonObjectParseError))
            {
                return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(jsonObjectParseError));
            }
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(new FlattenCollectionItemsPathElement(functionName, lambdaPredicate, jsonFunctionContext.VariablesManager, lineInfo));
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateReverseCollectionItemsPathElement(
        IParsedSimpleValue parsedSimpleValue, string functionName, IReadOnlyList<IExpressionItemBase> functionParameters, IJsonLineInfo? lineInfo)
    {
        if (functionParameters.Count > 0)
        {
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(
                new JsonObjectParseError($"Function [{functionName}] does not expect any parameters.",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(functionParameters[0]))
            ));
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(new ReverseCollectionItemsPathElement(functionName, lineInfo));
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateWhereCollectionItemsPathElement(
        IParsedSimpleValue parsedSimpleValue, string functionName, IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<ILambdaExpressionFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("criteria", typeof(ILambdaExpressionFunction), true),
            parametersJsonFunctionContext, lineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(parametersParseResult.Errors);

        if (!JsonValueLookupHelpers.TryGetLambdaPredicateFromParameter(functionName, parametersParseResult.Value!, out var lambdaPredicate,
                out var jsonObjectParseError))
        {
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(jsonObjectParseError));
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(new WhereCollectionItemsPathElement(functionName, lambdaPredicate, jsonFunctionContext.VariablesManager, lineInfo));
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateSelectCollectionItemsPathElement(
        IParsedSimpleValue parsedSimpleValue, string functionName, IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);
        
        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<ILambdaExpressionFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("path", typeof(ILambdaExpressionFunction), true),
            parametersJsonFunctionContext, lineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(parametersParseResult.Errors);

        if (!JsonValueLookupHelpers.TryGetJsonPathLambdaFunctionFromParameter(functionName, parametersParseResult.Value!, out var jsonPathLambdaFunction,
                out var jsonObjectParseError))
        {
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(jsonObjectParseError));
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(new SelectCollectionItemsPathElement(functionName, jsonPathLambdaFunction, jsonFunctionContext.VariablesManager, lineInfo));
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateSelectFirstLastCollectionItemsPathElement(
        bool isSelectFirstItem,
        IParsedSimpleValue parsedSimpleValue, string functionName, IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<ILambdaExpressionFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("criteria", typeof(ILambdaExpressionFunction), false),
            parametersJsonFunctionContext, lineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(parametersParseResult.Errors);

        IPredicateLambdaFunction? lambdaPredicate = null;

        if (parametersParseResult.Value != null)
        {
            if (!JsonValueLookupHelpers.TryGetLambdaPredicateFromParameter(functionName, parametersParseResult.Value, out lambdaPredicate,
                    out var jsonObjectParseError))
            {
                return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(jsonObjectParseError));
            }
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(
            isSelectFirstItem ?
            new SelectFirstCollectionItemPathElement(functionName, lambdaPredicate, jsonFunctionContext.VariablesManager, lineInfo) :
            new SelectLastCollectionItemPathElement(functionName, lambdaPredicate, jsonFunctionContext.VariablesManager, lineInfo));
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateSelectCollectionItemPathElement(
        IParsedSimpleValue parsedSimpleValue, string functionName, IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);
      
        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, ILambdaExpressionFunction, IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("index", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("criteria", typeof(ILambdaExpressionFunction), false),
            new JsonFunctionParameterMetadata("isReverseSearch", typeof(IJsonFunction), false),
            parametersJsonFunctionContext, lineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(parametersParseResult.Errors);

        IPredicateLambdaFunction? lambdaPredicate = null;

        if (parametersParseResult.Value.parameter2 != null)
        {
            if (!JsonValueLookupHelpers.TryGetLambdaPredicateFromParameter(functionName, parametersParseResult.Value.parameter2, out lambdaPredicate,
                    out var jsonObjectParseError))
            {
                return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(CollectionExpressionHelpers.Create(jsonObjectParseError));
            }
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(
            new SelectCollectionItemPathElement(functionName, parametersParseResult.Value.parameter1!,
                lambdaPredicate, parametersParseResult.Value.parameter3!, parametersJsonFunctionContext.VariablesManager, lineInfo));
    }
}