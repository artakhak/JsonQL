using JsonQL.Compilation;
using JsonQL.JsonObjects;

namespace JsonQL.Query;

public interface IJsonValueQueryResult
{
    IParsedValue? ParsedValue { get; }
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
}

public class JsonValueQueryResult : IJsonValueQueryResult
{
    public JsonValueQueryResult(IParsedValue parsedValue)
    {
        ParsedValue = parsedValue;
    }

    public JsonValueQueryResult(IReadOnlyList<ICompilationErrorItem> compilationErrors)
    {
        CompilationErrors = compilationErrors;
    }

    /// <inheritdoc />
    public IParsedValue? ParsedValue { get; }

    /// <inheritdoc />
    public IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; } = Array.Empty<ICompilationErrorItem>();
}