using JsonQL.Compilation;
using JsonQL.Compilation.JsonValueTextGenerator;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver;

namespace JsonQL.Demos.CustomJsonQL.Compilation;

/// <summary>
/// Example of factory for creating <see cref="IJsonCompiler"/> that supports custom language constricts, such as new or replaced operators,
/// functions, etc.
/// </summary>
public class CustomJsonCompilerFactory: JsonCompilerFactory
{
    public CustomJsonCompilerFactory(TryResolveConstructorParameterValueDelegate tryResolveConstructorParameterValueDelegate,
        IStringFormatter? stringFormatter = null,
        Func<Type, bool>? resolvedTypeInstanceCanBeCached = null, ILog? logger = null) : base(tryResolveConstructorParameterValueDelegate, stringFormatter, resolvedTypeInstanceCanBeCached, logger)
    {
    }
}