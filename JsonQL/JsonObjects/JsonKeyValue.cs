namespace JsonQL.JsonObjects;

/// <inheritdoc />
public class JsonKeyValue : IJsonKeyValue
{
    /// <summary>
    /// Represents a key-value pair in a parsed JSON object. The class provides access to
    /// the key, value, and the parent JSON object it belongs to.
    /// </summary>
    public JsonKeyValue(string key, IParsedJson parent)
    {
        Key = key;
        Parent = parent;
    }

    /// <inheritdoc />
    public IParsedJson Parent { get; }

    /// <inheritdoc />
    public string Key { get; }

    /// <inheritdoc />
    public IParsedValue Value { get; set; } = null!;

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; set; }
}