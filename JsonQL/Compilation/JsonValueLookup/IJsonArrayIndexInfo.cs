using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents information about a specific JSON array index,
/// including its position within the array and optional line information for diagnostic purposes.
/// </summary>
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
    /// <summary>
    /// Represents information about the index of an array element within a JSON array, along with optional line and position information.
    /// </summary>
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