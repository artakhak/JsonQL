namespace JsonQL.JsonObjects;

public interface IJsonKeyValue
{
    IParsedJson Parent { get; }
    string Key { get; }
    IParsedValue Value { get; }
    IJsonLineInfo? LineInfo { get; }
}

public class JsonKeyValue : IJsonKeyValue
{
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