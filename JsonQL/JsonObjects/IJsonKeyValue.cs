namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a key-value pair within a JSON object.
/// This interface provides access to the key, its associated value, and metadata such as the parent JSON object and line information.
/// </summary>
public interface IJsonKeyValue
{
    /// <summary>
    /// Gets the parent parsed JSON object that contains this key-value pair.
    /// </summary>
    /// <remarks>
    /// The parent object represents the broader structure in which this key-value pair exists.
    /// This property allows navigation to the containing parsed JSON object.
    /// </remarks>
    IParsedJson Parent { get; }

    /// <summary>
    /// Gets the key associated with this key-value pair in the parsed JSON object.
    /// </summary>
    /// <remarks>
    /// The key serves as the unique identifier for this value within its parent JSON object, allowing direct access to its associated data.
    /// It must follow the JSON specification for keys, typically represented as a string.
    /// </remarks>
    string Key { get; }

    /// <summary>
    /// Gets or sets the parsed JSON value that is associated with the key in this key-value pair.
    /// </summary>
    /// <remarks>
    /// The value represents the data or object linked to the key within the containing JSON structure.
    /// This property allows access to and manipulation of the specific value associated with the key.
    /// </remarks>
    IParsedValue Value { get; }

    /// <summary>
    /// Gets or sets the line and position information for this key-value pair within the JSON document.
    /// </summary>
    /// <remarks>
    /// This property provides details about the location of the JSON element,
    /// including the line number and the position relative to the start of the line.
    /// Useful for debugging or error reporting related to the JSON structure.
    /// </remarks>
    IJsonLineInfo? LineInfo { get; }
}

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