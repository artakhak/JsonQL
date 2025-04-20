using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonObjects;

public interface IParsedValue
{
    /// <summary>
    /// Value path. To avoid the path getting out of sync when the json is modified, the value
    /// might be dynamically calculated from parent. Therefore, this method should not be called frequently. 
    /// </summary>
    /// <returns>Returns value path.</returns>
    IJsonPath GetPath();

    /// <summary>
    /// Unique Id identifying the value. Use <see cref="Guid.NewGuid"/> method in constructor of an implementation
    /// of <see cref="IParsedValue"/> to set this value.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Root json value that owns this value.
    /// </summary>
    IRootParsedValue RootParsedValue { get; }

    /// <summary>
    /// If the value of <see cref="ParentJsonValue"/> is not null, then the value of <see cref="JsonKeyValue"/> should be null, and vice versa.
    /// </summary>
    IParsedValue? ParentJsonValue { get; }

    /// <summary>
    /// If the value of <see cref="ParentJsonValue"/> is not null, then the value of <see cref="JsonKeyValue"/> should be null, and vice versa.
    /// </summary>
    IJsonKeyValue? JsonKeyValue { get; }

    /// <summary>
    /// If the value is not null, specifies the position of parsed value in json file. The value 
    /// </summary>
    IJsonLineInfo? LineInfo { get; }
}