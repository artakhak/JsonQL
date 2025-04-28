using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element that selects the first item in a collection within a JSON data structure.
/// This class provides functionality to evaluate and resolve the first item that satisfies a given predicate
/// in a collection, or the first item if no predicate is specified.
/// </summary>
public class SelectFirstCollectionItemPathElement : JsonValueCollectionItemSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction? _lambdaPredicate;

    /// <summary>
    /// Represents a path element that selects the first item in a JSON collection, optionally filtered
    /// by a predicate function. This class is used in JSON value lookups.
    /// </summary>
    /// <remarks>
    /// The selection behavior can be influenced by providing a lambda predicate function.
    /// This allows filtering within a JSON collection to determine the first matching item.
    /// </remarks>
    public SelectFirstCollectionItemPathElement(
        IPredicateLambdaFunction? lambdaPredicate,
        IJsonLineInfo? lineInfo) : base(JsonValuePathFunctionNames.FirstCollectionItemSelectorFunction, lineInfo)
    {
        _lambdaPredicate = lambdaPredicate;
    }

    /// <inheritdoc />
    protected override IParseResult<ISingleItemJsonValuePathLookupResult> SelectCollectionItem(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        IParsedValue? selectedValue = null;
        for (var i = 0; i < parenParsedValues.Count; ++i)
        {
            var parsedValue = parenParsedValues[i];
            var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, i);

            if (_lambdaPredicate != null)
            {
                var predicateExpressionResult = _lambdaPredicate.LambdaExpressionFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, itemContextData);

                if (predicateExpressionResult.Errors.Count > 0)
                    return new ParseResult<ISingleItemJsonValuePathLookupResult>(predicateExpressionResult.Errors);

                if (!(predicateExpressionResult.Value ?? false))
                    continue;
            }

            selectedValue = parsedValue;
            break;
        }
        
        return new ParseResult<ISingleItemJsonValuePathLookupResult>(
            selectedValue != null ? SingleItemJsonValuePathLookupResult.CreateForValidPath(selectedValue) :
            SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
    }

    /// <inheritdoc />
    public IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData)
    {
        return LambdaFunctionParameterResolverHelpers.TryEvaluateLambdaFunctionParameterValue(this._lambdaPredicate, variableName, contextData);
    }
}