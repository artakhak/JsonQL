using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a parsed array value within a JSON structure, inheriting behavior for manipulating and interacting
/// with collections of parsed values. This class is a concrete implementation built on top of the abstract
/// <see cref="ParsedArrayValueAbstr"/> type.
/// </summary>
/// <remarks>
/// The class serves as a concrete representation of an array parsed from JSON. It provides mechanisms for
/// accessing the root parsed value, managing the parent hierarchy, and associating optional key-value pairs.
/// Instances of this class are typically constructed using a visitor pattern for parsing operations.
/// </remarks>
public class ParsedArrayValue : ParsedArrayValueAbstr
{
    /// <summary>
    /// Represents a parsed array value derived from the JSON input, implemented as a class extending <see cref="ParsedArrayValueAbstr"/>.<br/>
    /// Encapsulates array elements within a JSON structure and ties them to a provided root parsed value and optional JSON key-value metadata.
    /// </summary>
    /// <remarks>
    /// This class facilitates operations that require parsing or manipulation of JSON arrays within the application's object model.<br/>
    /// It is constructed with references to a root parsed value, its visitor, parent parsed object, and any associated metadata (key-value pair).<br/>
    /// Instances of this class are used during JSON parsing and internal representation building.
    /// </remarks>
    public ParsedArrayValue(IParsedJsonVisitor parsedJsonVisitor, IRootParsedValue rootParsedValue, IParsedValue parentJsonValue, IJsonKeyValue? jsonKeyValue,
        IJsonPath? pathInReferencedJson) : 
        base(parsedJsonVisitor, parentJsonValue, jsonKeyValue, pathInReferencedJson)
    {
        RootParsedValue = rootParsedValue;
    }

    /// <inheritdoc />
    public override IRootParsedValue RootParsedValue { get; }
}