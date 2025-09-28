using JsonQL.Compilation;
using JsonQL.Demos.Examples;

namespace JsonQL.Demos.DocFiles.JsonPathFunctions.Last.Example;

public class Example : JsonCompilerExampleManagerForSuccessAbstr
{
    private readonly IJsonCompiler _jsonCompiler;

    public Example(IJsonCompiler jsonCompiler)
    {
        _jsonCompiler = jsonCompiler;
    }

    /// <inheritdoc />
    protected override ICompilationResult Compile()
    {
        var result = _jsonCompiler.Compile(
            new JsonTextData("Example", this.LoadExampleJsonFile("Example.json"),
            new JsonTextData("Data", this.LoadExampleJsonFile("Data.json"))));

        return result;
    }

    protected override bool SerializeOnlyTheLastParsedFile => true;
}
