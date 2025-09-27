namespace JsonQL.JsonObjects.JsonPath;

/// <inheritdoc />
public class JsonPropertyNamePathElement : IJsonPropertyNamePathElement
{
    /// <summary>
    /// Represents an element in a JSON path specifically defined by a property name.
    /// This class is used to define named property segments in a JSON path structure.
    /// </summary>
    public JsonPropertyNamePathElement(string name)
    {
        Name = name;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc />
    public bool Equals(IJsonPathElement jsonPathElement)
    {
        if (jsonPathElement is not JsonPropertyNamePathElement jsonPropertyNamePathElement)
            return false;

        return string.Equals(this.Name, jsonPropertyNamePathElement.Name, StringComparison.Ordinal);
    }
}