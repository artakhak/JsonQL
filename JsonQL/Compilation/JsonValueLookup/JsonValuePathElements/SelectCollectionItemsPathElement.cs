using JsonQL.JsonFunction;
using JsonQL.JsonFunction.JsonFunctions.AggregateFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

public class SelectCollectionItemsPathElement : JsonValueCollectionItemsSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IJsonPathLambdaFunction _jsonPathLambdaFunction;

    public SelectCollectionItemsPathElement(
        IJsonPathLambdaFunction jsonPathLambdaFunction,
        IJsonLineInfo? lineInfo) : base(JsonValuePathFunctionNames.SelectCollectionItemsFunction, lineInfo)
    {
        _jsonPathLambdaFunction = jsonPathLambdaFunction;
    }
    
    /// <inheritdoc />
    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        var selectedParsedValues = new List<IParsedValue>(parenParsedValues.Count);

        for (var i = 0; i < parenParsedValues.Count; ++i)
        {
            var parsedValue = parenParsedValues[i];
            var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, i);

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

        return new ParseResult<ICollectionJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(selectedParsedValues));
    }

    /// <inheritdoc />
    public IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData)
    {
        return LambdaFunctionParameterResolverHelpers.TryEvaluateLambdaFunctionParameterValue(this._jsonPathLambdaFunction, variableName, contextData);
    }
}