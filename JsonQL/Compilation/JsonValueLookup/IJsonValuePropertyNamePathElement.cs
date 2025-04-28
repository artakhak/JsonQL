namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents a specific element in a JSON value path that refers to a property by its name.
/// </summary>
public interface IJsonValuePropertyNamePathElement : IJsonValuePathElement
{
    /// Gets the name of the JSON property associated with this path element.
    /// This property represents the key used to identify a specific value within a
    /// JSON object. It facilitates the lookup and navigation of JSON structures within
    /// the context of JSON path evaluation or processing operations.
    string Name { get; }
}