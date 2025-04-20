using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

public class ReverseCollectionItemsPathElement : JsonValueCollectionItemsSelectorPathElementAbstr
{
    public ReverseCollectionItemsPathElement(
        IJsonLineInfo? lineInfo) : base(JsonValuePathFunctionNames.ReverseCollectionItemsSelectorFunction, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        return new ParseResult<ICollectionJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(
            parenParsedValues.Reverse().ToList()));
    }
}