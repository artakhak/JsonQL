using JsonQL.Compilation;
using JsonQL.JsonObjects;

namespace JsonQL.Query;

/// <inheritdoc />
public class JsonValueQueryResult : IJsonValueQueryResult
{
    /// <summary>
    /// Represents the result of a JSON value query operation.
    /// Provides access to the parsed JSON value or a collection of encountered compilation errors during query execution.
    /// </summary>
    /// <param name="parsedValue">Parsed JSON value.</param>
    public JsonValueQueryResult(IParsedValue parsedValue)
    {
        ParsedValue = parsedValue;
    }

    /// <summary>
    /// Represents the result of a JSON value query operation. Provides access to the parsed JSON value
    /// or a collection of encountered compilation errors during query execution.
    /// </summary>
    /// <param name="compilationErrors">Collection of compilation errors.</param>
    public JsonValueQueryResult(IReadOnlyList<ICompilationErrorItem> compilationErrors)
    {
        CompilationErrors = compilationErrors;
    }

    /// <inheritdoc />
    public IParsedValue? ParsedValue { get; }

    /// <inheritdoc />
    public IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; } = Array.Empty<ICompilationErrorItem>();
}