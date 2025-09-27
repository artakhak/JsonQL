using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

/// <inheritdoc />
public class CompilationErrorItem : ICompilationErrorItem
{
    /// <summary>
    /// Represents a compilation error item produced during the compilation process.
    /// </summary>
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