using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents the root-level implementation of a parsed JSON array value.
/// This class combines functionality provided by <see cref="ParsedArrayValueAbstr"/>
/// and implements <see cref="IRootParsedArrayValue"/> to handle root-specific operations on JSON arrays.
/// </summary>
/// <remarks>
/// The <c>RootParsedArrayValue</c> class is designed to manage and manipulate
/// parsed data structures that are JSON arrays at the root level. It handles
/// operations such as tracking parsed value additions, removals, and retrievals
/// by unique identifiers. This class also supports interaction with
/// parsed JSON through a visitor interface.
/// </remarks>
public class RootParsedArrayValue : ParsedArrayValueAbstr, IRootParsedArrayValue
{
    private readonly Dictionary<Guid, IParsedValue> _valueIdToValueMap = new();

    /// <summary>
    /// Represents the root-level parsed array value derived from the abstract parsed array value.
    /// This class is used to handle and manipulate root parsed JSON arrays, providing functionality
    /// to manage the parsed values, handle events for added or removed values, and interact with
    /// the parsed JSON structure through a visitor interface.
    /// </summary>
    public RootParsedArrayValue(IParsedJsonVisitor parsedJsonVisitor, string jsonTextIdentifier) : base(parsedJsonVisitor,null, null, null)
    {
        JsonTextIdentifier = jsonTextIdentifier;
    }

    /// <inheritdoc />
    public string JsonTextIdentifier { get; }

    /// <inheritdoc />
    public bool TryGetParsedValue(Guid valueId, [NotNullWhen(true)] out IParsedValue? parsedValue)
    {
        return _valueIdToValueMap.TryGetValue(valueId, out parsedValue);
    }

    /// <inheritdoc />
    public void ValueAdded(IParsedValue parsedValue)
    {
        _valueIdToValueMap[parsedValue.Id] = parsedValue;
    }

    /// <inheritdoc />
    public void ValueRemoved(IParsedValue parsedValue)
    {
        _valueIdToValueMap.Remove(parsedValue.Id);
    }

    /// <inheritdoc />
    public override IRootParsedValue RootParsedValue => this;
}