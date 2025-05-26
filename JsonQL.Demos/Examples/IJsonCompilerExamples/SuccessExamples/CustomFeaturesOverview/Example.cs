using JsonQL.Compilation;

namespace JsonQL.Demos.Examples.IJsonCompilerExamples.SuccessExamples.CustomFeaturesOverview;

public class Example : JsonCompilerExampleManagerAbstr
{
    private readonly IJsonCompiler _jsonCompiler;

    public Example(IJsonCompiler jsonCompiler)
    {
        _jsonCompiler = jsonCompiler;
    }

    /// <inheritdoc />
    protected override ICompilationResult Compile()
    {
        var companiesJsonTextData = new JsonTextData("Companies",
            this.LoadExampleJsonFile("Companies.json"));

        var result = _jsonCompiler.Compile(new JsonTextData("CustomFeaturesOverview",
            this.LoadExampleJsonFile("CustomFeaturesOverview.json"), companiesJsonTextData));
        return result;
    }
}