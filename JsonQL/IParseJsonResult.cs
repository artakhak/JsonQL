using JsonQL.Compilation;
using JsonQL.JsonObjects;

namespace JsonQL;

public interface IParseJsonResult
{
    IParsedJson? ParsedJson { get; }
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
}

public class ParseJsonResult : IParseJsonResult
{
    public ParseJsonResult(IParsedJson parsedJson)
    {
        ParsedJson = parsedJson;
    }

    public ParseJsonResult(IReadOnlyList<ICompilationErrorItem> compilationErrors)
    {
        CompilationErrors = compilationErrors;
    }

    /// <inheritdoc />
    public IParsedJson? ParsedJson { get; }

    /// <inheritdoc />
    public IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; } = Array.Empty<ICompilationErrorItem>();
}