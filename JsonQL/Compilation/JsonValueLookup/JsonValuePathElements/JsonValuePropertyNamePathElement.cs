using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <inheritdoc />
public class JsonValuePropertyNamePathElement : IJsonValuePropertyNamePathElement
{
    public JsonValuePropertyNamePathElement(string name, IJsonLineInfo? lineInfo = null)
    {
        Name = name;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }
}