using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element used to filter a collection of JSON values using a specified predicate function.
/// </summary>
public class WhereCollectionItemsPathElement : JsonValueCollectionItemsSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction _predicateLambdaFunction;

    /// <summary>
    /// Represents a path element used to filter collection items based on a predicate.
    /// </summary>
    /// <remarks>
    /// This class extends the functionality of <see cref="JsonValueCollectionItemsSelectorPathElementAbstr"/>
    /// and applies a predicate function to select specific items from a collection.
    /// </remarks>
    public WhereCollectionItemsPathElement(
        IPredicateLambdaFunction predicateLambdaFunction,
        IJsonLineInfo? lineInfo) : base(JsonValuePathFunctionNames.WhereCollectionItemsFunction, lineInfo)
    {
        _predicateLambdaFunction = predicateLambdaFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        var filteredParsedValues = new List<IParsedValue>(parenParsedValues.Count);

        for (var i = 0; i < parenParsedValues.Count; ++i)
        {
            var parsedValue = parenParsedValues[i];
            var itemContextData = new JsonFunctionEvaluationContextData(parsedValue, i);

            if (_predicateLambdaFunction != null)
            {
                var predicateExpressionResult = _predicateLambdaFunction.LambdaExpressionFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, itemContextData);

                if (predicateExpressionResult.Errors.Count > 0)
                    return new ParseResult<ICollectionJsonValuePathLookupResult>(predicateExpressionResult.Errors);

                if (!(predicateExpressionResult.Value ?? false))
                    continue;
            }

            filteredParsedValues.Add(parsedValue);
        }

        return new ParseResult<ICollectionJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(filteredParsedValues));
    }

    /// <inheritdoc />
    public IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData)
    {
        return LambdaFunctionParameterResolverHelpers.TryEvaluateLambdaFunctionParameterValue(this._predicateLambdaFunction, variableName, contextData);
    }
}