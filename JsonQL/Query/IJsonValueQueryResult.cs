using JsonQL.Compilation;
using JsonQL.JsonObjects;

namespace JsonQL.Query;

/// <summary>
/// Represents the result of a JSON value query operation.
/// This interface defines the structure for holding the outcome of a query
/// that processes JSON data. It provides information about the parsed value
/// and any compilation errors encountered during the operation.
/// </summary>
public interface IJsonValueQueryResult
{
    IParsedValue? ParsedValue { get; }
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
}

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