using JsonQL.Compilation;

namespace JsonQL.Demos.Examples;

public abstract class JsonCompilerExampleManagerAbstr : ExampleManagerAbstr
{
    /// <inheritdoc />
    protected override Task<object> GetJsonQlResultAsync()
    {
        return Task.FromResult<object>(this.Compile());
    }

    protected abstract ICompilationResult Compile();
}