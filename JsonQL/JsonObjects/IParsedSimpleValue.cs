using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a parsed JSON value that is a simple, non-composite type.
/// This interface is used to model simple JSON data types such as strings, numbers,
/// and boolean values. It includes properties that provide additional
/// information about the value, such as whether it is a string.
/// </summary>
public interface IParsedSimpleValue : IParsedValue
{
    /// <summary>
    /// Represents the value of a simple, non-composite JSON type.
    /// This property holds the actual content of the parsed JSON, which can be a string, number, or boolean.
    /// If the value is a string, it will be quoted when serialized. If null, it represents a "null" JSON value.
    /// </summary>
    string? Value { get; set; }

    /// <summary>
    /// If the value is true, the serialized value will be enclosed onm apostrophes. Example: "BusName": "DefaultMessageHandler".
    /// Otherwise, the value will not be enclosed in  apostrophes. Example: "RebusNumberOfWorkers": 10.
    /// </summary>
    public bool IsString { get; }
}

/// <summary>
/// Represents a parsed JSON value that is a simple, non-composite type.
/// This class is used to model simple JSON data types such as strings, numbers,
/// and boolean values. It provides metadata about the value, including whether it is
/// a string and its associated parent relationships within the JSON hierarchy.
/// </summary>
public class ParsedSimpleValue : ParsedValueAbstr, IParsedSimpleValue
{
    /// <summary>
    /// Represents a simple parsed value in a JSON structure. This class serves as an implementation of the
    /// `IParsedSimpleValue` interface and extends the base functionality provided by `ParsedValueAbstr`.
    /// A `ParsedSimpleValue` is used to encapsulate a value in a JSON structure, including its parent-child
    /// relationships, optional key-value pairs, and additional attributes such as whether the value is represented
    /// as a string.
    /// This class is commonly created during the evaluation or mutation of JSON data and is capable of representing
    /// primitive JSON values such as strings, numbers, and booleans, among others.
    /// </summary>
    public ParsedSimpleValue(IRootParsedValue rootParsedValue, IParsedValue? parentJsonValue, IJsonKeyValue? jsonKeyValue,
        IJsonPath? pathInReferencedJson,
        string? value, bool isString): 
        base(parentJsonValue, jsonKeyValue, pathInReferencedJson)
    {
        RootParsedValue = rootParsedValue;
        Value = value;
        IsString = isString;
    }

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public bool IsString { get; }

    /// <inheritdoc />
    public override IRootParsedValue RootParsedValue { get; }
}