using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element that flattens the elements of a collection and allows for optional filtering
/// using a predicate function within a JSON query framework. This path element is designed to process
/// collections by aggregating their items into a single-level structure.
/// </summary>
public class FlattenCollectionItemsPathElement : JsonValueCollectionItemsSelectorPathElementAbstr, IResolvesVariableValue
{
    private readonly IPredicateLambdaFunction? _lambdaPredicate;
    private readonly IVariablesManager _variablesManager;

    /// <summary>
    /// Represents a path element for flattening collection items in a JSON value lookup operation.
    /// </summary>
    /// <param name="selectorName">Selector name</param>
    /// <param name="lambdaPredicate">If the value is not null, a predicate that will filter out the collection items.</param>
    /// <param name="variablesManager">Variables manager used to register variables, and retrieve variable values.</param>
    /// <param name="lineInfo">Path expression position.</param>
    /// <remarks>
    /// This class serves as a specialized implementation for selecting and retrieving collection items
    /// from a JSON object during a JSON query execution. It supports filtering collection items
    /// using an optional lambda predicate function provided during initialization.
    /// Inherits from <see cref="JsonValueCollectionItemsSelectorPathElementAbstr"/>.
    /// </remarks>
    public FlattenCollectionItemsPathElement(string selectorName,
        IPredicateLambdaFunction? lambdaPredicate,
        IVariablesManager variablesManager,
        IJsonLineInfo? lineInfo) : base(selectorName, lineInfo)
    {
        _lambdaPredicate = lambdaPredicate;
        _variablesManager = variablesManager;
    }

    /// <inheritdoc />
    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        this._variablesManager.Register(this);

        try
        {
            var flattenedParsedValues = new List<IParsedValue>(parenParsedValues.Count);

            for (var i = 0; i < parenParsedValues.Count; ++i)
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
                            return new ParseResult<ICollectionJsonValuePathLookupResult>(predicateExpressionResult.Errors);

                        if (!(predicateExpressionResult.Value ?? false))
                            continue;
                    }
                    finally
                    {
                        this._variablesManager.UnregisterVariableValue(this, _lambdaPredicate.ParameterJsonFunction.Name);
                    }
                }

                if (parsedValue is IParsedArrayValue parsedArrayValue)
                {
                    flattenedParsedValues.AddRange(parsedArrayValue.Values);
                }
                else
                {
                    flattenedParsedValues.Add(parsedValue);
                }
            }

            return new ParseResult<ICollectionJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(flattenedParsedValues));
        }
        finally
        {
            this._variablesManager.UnRegister(this);
        }
    }
}