// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element used to select the last item of a JSON collection.
/// Additionally, it allows optional predicate-based filtering to evaluate which element in the collection qualifies.
/// </summary>
public class SelectLastCollectionItemPathElement : JsonValueCollectionItemSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction? _lambdaPredicate;
    private readonly IVariablesManager _variablesManager;

    /// <summary>
    /// Represents a path element responsible for selecting the last item from a collection in a JSON data structure.
    /// Implements a predicate lambda for filtering and optionally includes line information for JSON parsing context.
    /// </summary>
    public SelectLastCollectionItemPathElement(
        string selectorName,
        IPredicateLambdaFunction? lambdaPredicate,
        IVariablesManager variablesManager,
        IJsonLineInfo? lineInfo) : base(selectorName, lineInfo)
    {
        _lambdaPredicate = lambdaPredicate;
        _variablesManager = variablesManager;
    }

    /// <inheritdoc />
    protected override IParseResult<ISingleItemJsonValuePathLookupResult> SelectCollectionItem(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        IParsedValue? selectedValue = null;

        this._variablesManager.Register(this);
        try
        {
            for (var i = parenParsedValues.Count - 1; i >= 0; --i)
            {
                var parsedValue = parenParsedValues[i];
                var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, i);

                if (_lambdaPredicate != null)
                {
                    this._variablesManager.RegisterVariableValue(this, _lambdaPredicate.ParameterJsonFunction.Name, itemContextData.EvaluatedValue);

                    try
                    {
                        var predicateExpressionResult = _lambdaPredicate.LambdaExpressionFunction.EvaluateBooleanValue(rootParsedValue, compiledParentRootParsedValues, itemContextData);

                        if (predicateExpressionResult.Errors.Count > 0)
                            return new ParseResult<ISingleItemJsonValuePathLookupResult>(predicateExpressionResult.Errors);

                        if (!(predicateExpressionResult.Value ?? false))
                            continue;
                    }
                    finally
                    {
                        this._variablesManager.UnregisterVariableValue(this, _lambdaPredicate.ParameterJsonFunction.Name);
                    }
                }

                selectedValue = parsedValue;
                break;
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
