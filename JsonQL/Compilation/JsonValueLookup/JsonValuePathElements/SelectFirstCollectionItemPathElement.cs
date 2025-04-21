using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

public class SelectFirstCollectionItemPathElement : JsonValueCollectionItemSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction? _lambdaPredicate;

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