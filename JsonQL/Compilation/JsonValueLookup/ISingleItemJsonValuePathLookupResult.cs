using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// A lookup by a path that selects multiple items. Examples are items selected by paths like this:<br/>
/// "parent.Object1.Array1", "Object1.Array1", "Object1.Array1[1, 3]", "Object1.Array1.Where(x => x.EmployeeId==11).At(1)"<br/>
/// "Object1.Array1.Where(x => x.EmployeeId==11).First(1)", etc.
/// </summary>
public interface ISingleItemJsonValuePathLookupResult: IJsonValuePathLookupResult
{
    IParsedValue? ParsedValue { get; }

    /// <summary>
    /// If the value is true, path is valid. Otherwise, the path does not exist.
    /// </summary>
    bool IsValidPath { get; }
}

public class SingleItemJsonValuePathLookupResult : ISingleItemJsonValuePathLookupResult
{
    private SingleItemJsonValuePathLookupResult(bool isValidPath,IParsedValue? parsedValue)
    {
        IsValidPath = isValidPath;
        ParsedValue = parsedValue;
    }

    public static SingleItemJsonValuePathLookupResult CreateForValidPath(IParsedValue? parsedValue)
    {
        return new SingleItemJsonValuePathLookupResult(true, parsedValue);
    }

    public static SingleItemJsonValuePathLookupResult CreateForInvalidPath()
    {
        return new SingleItemJsonValuePathLookupResult(false, null);
    }


    /// <inheritdoc />
    public bool IsSingleItemLookup => true;

    public bool HasValue => this.ParsedValue != null;

    /// <inheritdoc />
    public IParsedValue? ParsedValue { get; }

    public bool IsValidPath { get; }
}