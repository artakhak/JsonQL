using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

public interface IRootParsedArrayValue : IRootParsedValue, IParsedArrayValue
{

}

public class RootParsedArrayValue : ParsedArrayValueAbstr, IRootParsedArrayValue
{
    private readonly Dictionary<Guid, IParsedValue> _valueIdToValueMap = new();

    public RootParsedArrayValue(IParsedJsonVisitor parsedJsonVisitor) : base(parsedJsonVisitor,null, null)
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