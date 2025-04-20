using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

public abstract class JsonValueCollectionItemsSelectorPathElementAbstr : IJsonValueCollectionItemsSelectorPathElement
{
    protected JsonValueCollectionItemsSelectorPathElementAbstr(string functionName, IJsonLineInfo? lineInfo)
    {
        FunctionName = functionName;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> Select(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        return SelectCollectionItems(parenParsedValues, rootParsedValue, compiledParentRootParsedValues);
    }

    protected abstract IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
    
    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc />
    public string FunctionName { get; }

    /// <inheritdoc />
    public bool SelectsSingleItem => false;
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"{FunctionName}(...)";
    }
}