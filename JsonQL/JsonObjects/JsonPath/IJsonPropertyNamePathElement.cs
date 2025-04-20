namespace JsonQL.JsonObjects.JsonPath;

public interface IJsonPropertyNamePathElement : IJsonPathElement
{
    string Name { get; }
}

/// <inheritdoc />
public class JsonPropertyNamePathElement : IJsonPropertyNamePathElement
{
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