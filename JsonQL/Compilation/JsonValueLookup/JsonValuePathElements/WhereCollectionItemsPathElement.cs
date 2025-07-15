// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element used to filter a collection of JSON values using a specified predicate function.
/// </summary>
public class WhereCollectionItemsPathElement : JsonValueCollectionItemsSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction _predicateLambdaFunction;
    private readonly IVariablesManager _variablesManager;

    /// <summary>
    /// Represents a path element used to filter collection items based on a predicate.
    /// </summary>
    /// <remarks>
    /// This class extends the functionality of <see cref="JsonValueCollectionItemsSelectorPathElementAbstr"/>
    /// and applies a predicate function to select specific items from a collection.
    /// </remarks>
    public WhereCollectionItemsPathElement(
        string selectorName,
        IPredicateLambdaFunction predicateLambdaFunction,
        IVariablesManager variablesManager,
        IJsonLineInfo? lineInfo) : base(selectorName, lineInfo)
    {
        _predicateLambdaFunction = predicateLambdaFunction;
        _variablesManager = variablesManager;
    }

    /// <inheritdoc />
    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        this._variablesManager.Register(this);

        try
        {
            var filteredParsedValues = new List<IParsedValue>(parenParsedValues.Count);

            for (var i = 0; i < parenParsedValues.Count; ++i)
            {
                var parsedValue = parenParsedValues[i];
                var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, i);
                this._variablesManager.RegisterVariableValue(this, _predicateLambdaFunction.ParameterJsonFunction.Name, itemContextData.EvaluatedValue);

                try
                {
                    var predicateExpressionResult = _predicateLambdaFunction.LambdaExpressionFunction.EvaluateBooleanValue(rootParsedValue, compiledParentRootParsedValues, itemContextData);

                    if (predicateExpressionResult.Errors.Count > 0)
                        return new ParseResult<ICollectionJsonValuePathLookupResult>(predicateExpressionResult.Errors);

                    if (!(predicateExpressionResult.Value ?? false))
                        continue;

                    filteredParsedValues.Add(parsedValue);
                }
                finally
                {
                    this._variablesManager.UnregisterVariableValue(this, _predicateLambdaFunction.ParameterJsonFunction.Name);
                }
            }

            return new ParseResult<ICollectionJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(filteredParsedValues));
        }
        finally
        {
            this._variablesManager.UnRegister(this);
        }
    }
}