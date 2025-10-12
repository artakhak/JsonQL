using JsonQL.Compilation;
using JsonQL.Demos.Examples;

namespace JsonQL.Demos.DocFiles.Operators.OtherOperators.Assert.Examples;

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
        var dataJsonTextData = new JsonTextData("Data", this.LoadExampleJsonFile("Data.json"));
       
        var result = _jsonCompiler.Compile(new JsonTextData("Examples",
            this.LoadExampleJsonFile("Examples.json"), dataJsonTextData));

        return result;
    }

    /// <inheritdoc />
    protected override bool SerializeOnlyTheLastParsedFile => true;
}