using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

public interface ICompilationErrorItem
{
    /// <summary>
    /// Json text identifier.
    /// </summary>
    string JsonTextIdentifier { get; }

    /// <summary>
    /// Error line info.
    /// </summary>
    IJsonLineInfo? LineInfo { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    string ErrorMessage { get; }
}

public class CompilationErrorItem : ICompilationErrorItem
{
    public CompilationErrorItem(string jsonTextIdentifier, string errorMessage, IJsonLineInfo? lineInfo)
    {
        JsonTextIdentifier = jsonTextIdentifier;
        ErrorMessage = errorMessage;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public string JsonTextIdentifier { get; }

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }
    
    /// <inheritdoc />
    public string ErrorMessage { get; }
}