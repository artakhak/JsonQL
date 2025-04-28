using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents a JSON path element that is responsible for selecting
/// items from a collection in a JSON structure.
/// </summary>
public interface IJsonValueCollectionItemsSelectorPathElement : IJsonValuePathElement
{
    string FunctionName { get; }
    
    /// <summary>
    /// If the value is true, always selects a single item from. Example: "Object1.Array1.First(x => x > 10)",
    /// otherwise, the path element might select multiple items from an array. Example: "Object1.Array1.Where(x => x > 10)",
    /// </summary>
    bool SelectsSingleItem { get; }

    /// <summary>
    /// Selects items from <param name="parenParsedValues"></param>
    /// </summary>
    IParseResult<IJsonValuePathLookupResult> Select(IReadOnlyList<IParsedValue> parenParsedValues,
        IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
}