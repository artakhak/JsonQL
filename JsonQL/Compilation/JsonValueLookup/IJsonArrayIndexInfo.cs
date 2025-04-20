using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

public interface IJsonArrayIndexInfo
{
    /// <summary>
    /// Line info.
    /// </summary>
    IJsonLineInfo? LineInfo { get; }

    /// <summary>
    /// Index.
    /// </summary>
    int Index { get; }
}

/// <inheritdoc />
public class JsonArrayIndexInfo : IJsonArrayIndexInfo
{
    public JsonArrayIndexInfo(int index, IJsonLineInfo? lineInfo)
    {
        Index = index;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public int Index { get; }

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Index.ToString();
    }
}