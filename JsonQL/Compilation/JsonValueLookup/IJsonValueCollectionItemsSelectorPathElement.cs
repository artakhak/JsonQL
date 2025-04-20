using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;


public interface IJsonValueCollectionItemsSelectorPathElement : IJsonValuePathElement
{
    string FunctionName { get; }
    
    /// <summary>
    /// If the value is true, always selects a single item from. Example: "Object1.Array1.first()",
    /// otherwise, the path element might select multiple items from array.  Example: "Object1.Array1.where(value > 10)",
    /// </summary>
    bool SelectsSingleItem { get; }

    /// <summary>
    /// Selects items from <param name="parenParsedValues"></param>
    /// </summary>
    IParseResult<IJsonValuePathLookupResult> Select(IReadOnlyList<IParsedValue> parenParsedValues,
        IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
}