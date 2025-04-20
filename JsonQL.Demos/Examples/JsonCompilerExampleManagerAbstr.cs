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

    /// <inheritdoc />
    protected override string SerializeResult(object result)
    {
        if (result is not ICompilationResult compilationResult)
            throw new ArgumentException($"The value is expected to be of type [{typeof(ICompilationResult)}]. Actual type was [{result.GetType()}].",
                nameof(result));

        return CompilationResultSerializerAmbientContext.Context.Serialize(compilationResult, 
            x => 
                // Lets output only the most recently compiled file
                x.TextIdentifier == compilationResult.CompiledJsonFiles[^1].TextIdentifier);
    }
}