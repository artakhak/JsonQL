using JsonQL.Compilation;

namespace JsonQL.Demos.Examples.IJsonCompilerExamples.FailureExamples.MissingClosingBracesError;

public class Example : JsonCompilerExampleManagerForFailureAbstr
{
    private readonly IJsonCompiler _jsonCompiler;

    public Example(IJsonCompiler jsonCompiler)
    {
        _jsonCompiler = jsonCompiler;
    }

    /// <inheritdoc />
    protected override ICompilationResult Compile()
    {
        var result = _jsonCompiler.Compile(new JsonTextData("Example",
            this.LoadExampleJsonFile("Example.json")));
        return result;
    }
}