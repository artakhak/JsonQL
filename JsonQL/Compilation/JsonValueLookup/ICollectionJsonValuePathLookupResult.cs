using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// A lookup by a path that selects multiple items. Examples are:<br/>
/// "parent.Object1.Array1.Flatten()", "parent.Object1.Array1.Flatten().Where(x => x.EmployeeId==11)",<br/>
/// "parent.Object1.Array1.Flatten().Reverse()", etc.
/// </summary>
public interface ICollectionJsonValuePathLookupResult: IJsonValuePathLookupResult
{
    IReadOnlyList<IParsedValue> ParsedValues { get; }
}

public class CollectionJsonValuePathLookupResult : ICollectionJsonValuePathLookupResult
{
    public CollectionJsonValuePathLookupResult(IReadOnlyList<IParsedValue> parsedValue)
    {
        ParsedValues = parsedValue;
    }

    /// <inheritdoc />
    public bool IsSingleItemLookup => false;

    public bool HasValue => ParsedValues.Count > 0;

    /// <inheritdoc />
    public IReadOnlyList<IParsedValue> ParsedValues { get; }
}