using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

public interface IJsonValuePath
{
    IJsonLineInfo? LineInfo { get; }
    IReadOnlyList<IJsonValuePathElement> Path { get; }
}

/// <inheritdoc />
public class JsonValuePath : IJsonValuePath
{
    public JsonValuePath(List<IJsonValuePathElement> path)
    {
        Path = path;
    }

    public IJsonLineInfo? LineInfo
    {
        get
        {
            if (Path.Count == 0)
                return null;

            return Path[0].LineInfo;
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<IJsonValuePathElement> Path { get; }

    /// <inheritdoc />
    public override string ToString() => string.Join(JsonOperatorNames.JsonPathSeparator, Path);
}