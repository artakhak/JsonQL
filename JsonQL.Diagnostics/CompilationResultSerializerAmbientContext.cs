using JsonQL.Compilation;
using JsonQL.Query;
using OROptimizer;

namespace JsonQL.Diagnostics;

public class CompilationResultSerializerAmbientContext : AmbientContext<ICompilationResultSerializer, NullCompilationResultSerializer>
{

}

/// <summary>
/// Null implementation used in <see cref="CompilationResultSerializerAmbientContext"/>.
/// Will be replaced with real implementation on application start.
/// </summary>
public class NullCompilationResultSerializer : ICompilationResultSerializer
{
    /// <inheritdoc />
    public string Serialize(ICompilationResult compilationResult, Func<ICompiledJsonData, bool> compiledJsonDataShouldBeIncluded)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string Serialize(IJsonValueQueryResult jsonValueQueryResult)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string Serialize(IObjectQueryResult objectQueryResult)
    {
        throw new NotImplementedException();
    }
}