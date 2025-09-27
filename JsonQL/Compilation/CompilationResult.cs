namespace JsonQL.Compilation;

/// <inheritdoc />
public class CompilationResult : ICompilationResult
{
    /// <summary>
    /// Represents the result of a compilation process, including compiled JSON files and any compilation errors.
    /// </summary>
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