// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a specific path element in a JSON value path used to select items from a collection.
/// This class provides functionality for filtering or extracting collection items based on a
/// lambda function and evaluating potential variable values.
/// </summary>
public class SelectCollectionItemsPathElement : JsonValueCollectionItemsSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IJsonPathLambdaFunction _jsonPathLambdaFunction;
    private readonly IVariablesManager _variablesManager;

    /// <summary>
    /// Represents a path element in a JSON value lookup that selects items from a collection
    /// based on a specified lambda function.
    /// </summary>
    public SelectCollectionItemsPathElement(
        string selectorName,
        IJsonPathLambdaFunction jsonPathLambdaFunction,
        IVariablesManager variablesManager,
        IJsonLineInfo? lineInfo) : base(selectorName, lineInfo)
    {
        _jsonPathLambdaFunction = jsonPathLambdaFunction;
        _variablesManager = variablesManager;
    }

    /// <inheritdoc />
    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        this._variablesManager.Register(this);

        try
        {
            var selectedParsedValues = new List<IParsedValue>(parenParsedValues.Count);

            for (var i = 0; i < parenParsedValues.Count; ++i)
            {
                var parsedValue = parenParsedValues[i];
                var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, i);

                this._variablesManager.RegisterVariableValue(this, this._jsonPathLambdaFunction.ParameterJsonFunction.Name, itemContextData.EvaluatedValue);

                try
                {
                    var pathParsedValuesResult = _jsonPathLambdaFunction.LambdaExpressionFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, itemContextData);

                    if (pathParsedValuesResult.Errors.Count > 0)
                        return new ParseResult<ICollectionJsonValuePathLookupResult>(pathParsedValuesResult.Errors);

                    if (pathParsedValuesResult.Value != null)
                    {
                        var parsedValuesResult = pathParsedValuesResult.Value.GetResultAsParsedValuesList(false, this.LineInfo);

                        if (parsedValuesResult.Errors.Count > 0)
                            return new ParseResult<ICollectionJsonValuePathLookupResult>(parsedValuesResult.Errors);

                        if (parsedValuesResult.Value == null || parsedValuesResult.Value.Count == 0)
                            continue;

                        selectedParsedValues.AddRange(parsedValuesResult.Value);
                    }
                }
                finally
                {
                    this._variablesManager.UnregisterVariableValue(this, this._jsonPathLambdaFunction.ParameterJsonFunction.Name);
                }
            }

            return new ParseResult<ICollectionJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(selectedParsedValues));
        }
        finally
        {
            this._variablesManager.UnRegister(this);
        }
    }
}
