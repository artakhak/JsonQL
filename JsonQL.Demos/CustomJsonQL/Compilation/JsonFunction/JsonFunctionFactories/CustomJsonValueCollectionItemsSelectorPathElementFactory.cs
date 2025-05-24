using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;
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

        return false;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValueCollectionItemsSelectorPathElement> Create(IParsedSimpleValue parsedSimpleValue, IBracesExpressionItem bracesExpressionItem, string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        return _defaultJsonValueCollectionItemsSelectorPathElementFactory.Create(parsedSimpleValue, bracesExpressionItem, functionName, jsonFunctionContext, lineInfo);
    }
}