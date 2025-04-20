namespace JsonQL.Compilation;

public interface ICompilationResult
{
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }

    /// <summary>
    /// List of compiled json files. If <see cref="CompilationErrors"/> is empty, the list is not empty.
    /// Otherwise, the list might be empty, or might have some items (for example if some parent Json files were compiled, but the main
    /// file failed to compile).
    /// </summary>
    IReadOnlyList<ICompiledJsonData> CompiledJsonFiles { get; }
}

/// <inheritdoc />
public class CompilationResult : ICompilationResult
{
    public CompilationResult(IReadOnlyList<ICompiledJsonData> compiledJsonFiles, IReadOnlyList<ICompilationErrorItem> compilationErrors)
    {
        CompiledJsonFiles = compiledJsonFiles;
        CompilationErrors = compilationErrors;
    }

    /// <inheritdoc />
    public IReadOnlyList<ICompiledJsonData> CompiledJsonFiles { get; }

    /// <inheritdoc />
    public IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
}