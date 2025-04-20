using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

public interface IJsonValuePathElement
{
    IJsonLineInfo? LineInfo { get; }
}
