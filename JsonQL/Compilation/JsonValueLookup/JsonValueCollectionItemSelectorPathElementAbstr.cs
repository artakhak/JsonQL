using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;


public abstract class JsonValueCollectionItemSelectorPathElementAbstr : IJsonValueCollectionItemsSelectorPathElement
{
    protected JsonValueCollectionItemSelectorPathElementAbstr(string functionName, IJsonLineInfo? lineInfo)
    {
        FunctionName = functionName;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> Select(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        return SelectCollectionItem(parenParsedValues, rootParsedValue, compiledParentRootParsedValues);
    }

    protected abstract IParseResult<ISingleItemJsonValuePathLookupResult> SelectCollectionItem(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc />
    public string FunctionName { get; }

    /// <inheritdoc />
    public bool SelectsSingleItem => true;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{FunctionName}(...)";
    }
}