using JsonQL.JsonFunction;
using JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

public class SelectCollectionItemPathElement : JsonValueCollectionItemSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IJsonFunction _itemIndex;
    private readonly IPredicateLambdaFunction? _lambdaPredicate;
    private readonly IJsonFunction? _isReverseSearch;

    public SelectCollectionItemPathElement(
        IJsonFunction itemIndex,
        IPredicateLambdaFunction? lambdaPredicate,
        IJsonFunction? isReverseSearch,
        IJsonLineInfo? lineInfo) : base(JsonValuePathFunctionNames.CollectionItemSelectorFunction, lineInfo)
    {
        _itemIndex = itemIndex;
        _lambdaPredicate = lambdaPredicate;
        _isReverseSearch = isReverseSearch;
    }

    /// <inheritdoc />
    protected override IParseResult<ISingleItemJsonValuePathLookupResult> SelectCollectionItem(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        var itemIndexResult = _itemIndex.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, null);

        if (itemIndexResult.Errors.Count > 0)
            return new ParseResult<ISingleItemJsonValuePathLookupResult>(itemIndexResult.Errors);

        if (itemIndexResult.Value == null || !JsonFunctionHelpers.TryConvertValueToJsonComparable(itemIndexResult.Value, TypeCode.Double, out var jsonComparable) ||
            jsonComparable.Value is not double doubleValue)
        {
            return new ParseResult<ISingleItemJsonValuePathLookupResult>(CollectionExpressionHelpers.Create(
                new JsonObjectParseError("Failed to convert the expression to numeric value.", _itemIndex.LineInfo)
            ));
        }

        var index = (int)doubleValue;

        if (index < 0 || index >= parenParsedValues.Count)
        {
            return new ParseResult<ISingleItemJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
        }

        bool isReverseSearch = false;

        if (_isReverseSearch != null)
        {
            var isReverseSearchResult = _isReverseSearch.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, null);

            if (isReverseSearchResult.Errors.Count > 0)

                return new ParseResult<ISingleItemJsonValuePathLookupResult>(isReverseSearchResult.Errors);

            if (isReverseSearchResult.Value == null || !JsonFunctionHelpers.TryConvertValueToJsonComparable(isReverseSearchResult.Value, TypeCode.Boolean, out jsonComparable) ||
                jsonComparable.Value is not bool booleanValue)
            {
                return new ParseResult<ISingleItemJsonValuePathLookupResult>(
                    CollectionExpressionHelpers.Create(
                    new JsonObjectParseError("Failed to convert the expression to a boolean value.", _isReverseSearch.LineInfo)));
            }

            isReverseSearch = booleanValue;
        }

        var currentIndex = 0;
        var indexIncrement = 1;

        if (isReverseSearch)
        {
            currentIndex = parenParsedValues.Count - 1;
            indexIncrement = -1;
        }

        var evaluatedValuesCount = 0;
        var countOfItemsMatchingCriteria = 0;

        IParsedValue? selectedValue = null;
        while (evaluatedValuesCount < parenParsedValues.Count)
        {
            var parsedValue = parenParsedValues[currentIndex];
            var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, currentIndex);
            
            if (_lambdaPredicate != null)
            {
                var predicateExpressionResult = _lambdaPredicate.LambdaExpressionFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, itemContextData);

                if (predicateExpressionResult.Errors.Count > 0)
                    return new ParseResult<ISingleItemJsonValuePathLookupResult>(predicateExpressionResult.Errors);

                if (!(predicateExpressionResult.Value ?? false))
                {
                    ++evaluatedValuesCount;
                    currentIndex += indexIncrement;
                    continue;
                }
            }

            if (countOfItemsMatchingCriteria == index)
            {
                selectedValue = parsedValue;
                break;
            }

            ++countOfItemsMatchingCriteria;
            ++evaluatedValuesCount;
            currentIndex += indexIncrement;
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