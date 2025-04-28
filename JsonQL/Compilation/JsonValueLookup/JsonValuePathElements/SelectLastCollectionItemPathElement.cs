using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element used to select the last item of a JSON collection.
/// Additionally, it allows optional predicate-based filtering to evaluate which element in the collection qualifies.
/// </summary>
public class SelectLastCollectionItemPathElement : JsonValueCollectionItemSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction? _lambdaPredicate;

    /// <summary>
    /// Represents a path element responsible for selecting the last item from a collection in a JSON data structure.
    /// Implements a predicate lambda for filtering and optionally includes line information for JSON parsing context.
    /// </summary>
    public SelectLastCollectionItemPathElement(
        IPredicateLambdaFunction? lambdaPredicate,
        IJsonLineInfo? lineInfo) : base(JsonValuePathFunctionNames.LastCollectionItemSelectorFunction, lineInfo)
    {
        _lambdaPredicate = lambdaPredicate;
    }

    /// <inheritdoc />
    protected override IParseResult<ISingleItemJsonValuePathLookupResult> SelectCollectionItem(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        IParsedValue? selectedValue = null;

        for (var i = parenParsedValues.Count - 1; i >= 0; --i)
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