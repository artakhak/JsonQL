namespace JsonQL.Compilation;

/// <inheritdoc />
public class CompilationResultMapper: ICompilationResultMapper
{
    /// <inheritdoc />
    public ICompilationResult Map(IReadOnlyList<string> compiledFileIdentifiers, ICompilationResult compilationResult)
    {
        return compilationResult;
    }
}