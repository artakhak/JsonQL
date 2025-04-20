using JsonQL.Compilation;
using JsonQL.Diagnostics;
using JsonQL.Extensions.Query;
using OROptimizer;

namespace JsonQL.Demos;

public class CompilationResultSerializerAmbientContext : AmbientContext<ICompilationResultSerializer, NullCompilationResultSerializer>
{

}

/// <summary>
/// Null implementation used in <see cref="CompilationResultSerializerAmbientContext"/>.
/// Will be replaced with real implementation on application start.
/// </summary>
public class NullCompilationResultSerializer : ICompilationResultSerializer
{
    public string Serialize(ICompilationResult compilationResult, Func<ICompiledJsonData, bool> compiledJsonDataShouldBeIncluded)
    {
        throw new NotImplementedException();
    }

    public string Serialize(IJsonValueQueryResult jsonValueQueryResult)
    {
        throw new NotImplementedException();
    }
}