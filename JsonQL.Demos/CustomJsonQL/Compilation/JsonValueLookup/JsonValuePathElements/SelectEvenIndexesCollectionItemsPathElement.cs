using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

public class SelectEvenIndexesCollectionItemsPathElement: JsonValueCollectionItemsSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction _predicateLambdaFunction;
    private readonly IVariablesManager _variablesManager;

    public SelectEvenIndexesCollectionItemsPathElement(string selectorName,
        IPredicateLambdaFunction predicateLambdaFunction,
        IVariablesManager variablesManager,
        IJsonLineInfo? lineInfo) : base(selectorName, lineInfo)
    {
        _predicateLambdaFunction = predicateLambdaFunction;
        _variablesManager = variablesManager;
    }

    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parentParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        this._variablesManager.Register(this);

        try
        {
            var filteredParsedValues = new List<IParsedValue>(parentParsedValues.Count);

            for (var i = 0; i < parentParsedValues.Count; i += 2)
            {
                var parsedValue = parentParsedValues[i];
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
