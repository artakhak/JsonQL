// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element in a JSON value path that is used to select an item
/// from a collection based on certain criteria.
/// </summary>
/// <remarks>
/// This class is an implementation of <see cref="JsonValueCollectionItemSelectorPathElementAbstr"/>
/// and supports the selection of collection items through index, optional lambda predicate,
/// and optional reverse search functionality.
/// </remarks>
/// <seealso cref="JsonValueCollectionItemSelectorPathElementAbstr"/>
/// <seealso cref="IResolvesVariableValue"/>
public class SelectCollectionItemPathElement : JsonValueCollectionItemSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IJsonFunction _itemIndex;
    private readonly IPredicateLambdaFunction? _lambdaPredicate;
    private readonly IJsonFunction? _isReverseSearch;
    private readonly IVariablesManager _variablesManager;

    /// <summary>
    /// Represents a path element that is responsible for selecting an element from a JSON collection
    /// based on a specified index, an optional lambda predicate for filtering, and an optional reverse search.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="JsonValueCollectionItemSelectorPathElementAbstr" /> and implements <see cref="IResolvesVariableValue" />.
    /// It utilizes a function name "At" to specify the collection item selector behavior.
    /// </remarks>
    public SelectCollectionItemPathElement(
        string selectorName,
        IJsonFunction itemIndex,
        IPredicateLambdaFunction? lambdaPredicate,
        IJsonFunction? isReverseSearch,
        IVariablesManager variablesManager,
        IJsonLineInfo? lineInfo) : base(selectorName, lineInfo)
    {
        _itemIndex = itemIndex;
        _lambdaPredicate = lambdaPredicate;
        _isReverseSearch = isReverseSearch;
        _variablesManager = variablesManager;
    }

    /// <inheritdoc />
    protected override IParseResult<ISingleItemJsonValuePathLookupResult> SelectCollectionItem(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        this._variablesManager.Register(this);

        try
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
                    this._variablesManager.RegisterVariableValue(this, _lambdaPredicate.ParameterJsonFunction.Name, itemContextData.EvaluatedValue);

                    try
                    {
                        var predicateExpressionResult = _lambdaPredicate.LambdaExpressionFunction.EvaluateBooleanValue(rootParsedValue, compiledParentRootParsedValues, itemContextData);

                        if (predicateExpressionResult.Errors.Count > 0)
                            return new ParseResult<ISingleItemJsonValuePathLookupResult>(predicateExpressionResult.Errors);

                        if (!(predicateExpressionResult.Value ?? false))
                        {
                            ++evaluatedValuesCount;
                            currentIndex += indexIncrement;
                            continue;
                        }
                    }
                    finally
                    {
                        this._variablesManager.UnregisterVariableValue(this, _lambdaPredicate.ParameterJsonFunction.Name);
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
        finally
        {
            this._variablesManager.UnRegister(this);
        }
    }
}
