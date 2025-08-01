using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonValueLookup;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonValueLookup.JsonValuePathElements;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// Provides a custom factory for creating instances of <see cref="IJsonValueCollectionItemsSelectorPathElement"/>
/// based on specified parameters like parsed values, function context, and function name.
/// Custom factory for creating instances of <see cref="IJsonValueCollectionItemsSelectorPathElement"/>.
/// Look at the implementations of <see cref="IJsonValueCollectionItemsSelectorPathElement"/>, such as
/// <see cref="WhereCollectionItemsPathElement"/> for examples.
/// </summary>
public class CustomJsonValueCollectionItemsSelectorPathElementFactory: IJsonValueCollectionItemsSelectorPathElementFactory
{
    private readonly IJsonValueCollectionItemsSelectorPathElementFactory _defaultJsonValueCollectionItemsSelectorPathElementFactory;
    private IJsonFunctionFromExpressionParser? _jsonFunctionFromExpressionParser;

    public CustomJsonValueCollectionItemsSelectorPathElementFactory(IJsonValueCollectionItemsSelectorPathElementFactory defaultJsonValueCollectionItemsSelectorPathElementFactory)
    {
        _defaultJsonValueCollectionItemsSelectorPathElementFactory = defaultJsonValueCollectionItemsSelectorPathElementFactory;
    }

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
        if (_defaultJsonValueCollectionItemsSelectorPathElementFactory.IsJsonValueCollectionItemsSelectorFunction(functionName))
            return true;

        if (functionName == CustomJsonValuePathFunctionNames.SelectEvenIndexesCollectionItemsSelectorFunction)
            return true;

        return false;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValueCollectionItemsSelectorPathElement> Create(IParsedSimpleValue parsedSimpleValue, IBracesExpressionItem bracesExpressionItem, string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        var functionNameLiteralExpression = bracesExpressionItem.NameLiteral;

        if (functionNameLiteralExpression == null)
        {
            // This will never happen, however we still should do a null check.
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>([
                new JsonObjectParseError("Function name is missing",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem))
            ]);
        }

        var functionParameters = bracesExpressionItem.Parameters;

        if (functionName == CustomJsonValuePathFunctionNames.SelectEvenIndexesCollectionItemsSelectorFunction)
            return CreateSelectEvenIndexesCollectionItemsPathElement(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);

        if (functionName == CustomJsonValuePathFunctionNames.SelectSecondItemCollectionItemSelectorFunction)
            return CreateSelectSecondCollectionItemsPathElement(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, lineInfo);


        return _defaultJsonValueCollectionItemsSelectorPathElementFactory.Create(parsedSimpleValue, bracesExpressionItem, functionName, jsonFunctionContext, lineInfo);
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateSelectEvenIndexesCollectionItemsPathElement(
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
            return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>([jsonObjectParseError]);
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(new SelectEvenIndexesCollectionItemsPathElement(functionName, lambdaPredicate, jsonFunctionContext.VariablesManager, lineInfo));
    }

    private IParseResult<IJsonValueCollectionItemsSelectorPathElement> CreateSelectSecondCollectionItemsPathElement(
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
                return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>([jsonObjectParseError]);
            }
        }

        return new ParseResult<IJsonValueCollectionItemsSelectorPathElement>(
            new SelectSecondCollectionItemPathElement(functionName, lambdaPredicate, jsonFunctionContext.VariablesManager, lineInfo));
    }
}
