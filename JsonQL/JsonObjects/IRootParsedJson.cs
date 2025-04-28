using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents the root object of a parsed JSON object model.
/// </summary>
/// <remarks>
/// This interface combines the capabilities of <see cref="IRootParsedValue"/> and <see cref="IParsedJson"/>,
/// serving as the root type in the parsed JSON hierarchy.
/// </remarks>
public interface IRootParsedJson : IRootParsedValue, IParsedJson
{

}

public class RootParsedJson: ParsedJsonAbstr, IRootParsedJson
{
    private readonly Dictionary<Guid, IParsedValue> _valueIdToValueMap = new();

    /// <summary>
    /// Represents the root parsed structure of JSON data. This class serves as
    /// the entry point for accessing and manipulating JSON content parsed into a
    /// hierarchical structure.
    /// </summary>
    /// <remarks>
    /// This class is derived from <see cref="ParsedJsonAbstr"/> and implements the
    /// <see cref="IRootParsedJson"/> interface. It provides methods to retrieve,
    /// add, and remove parsed values, as well as access the root value of the parsed JSON.
    /// </remarks>
    public RootParsedJson(IParsedJsonVisitor parsedJsonVisitor) : base(parsedJsonVisitor,null, null)
    {
    }

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