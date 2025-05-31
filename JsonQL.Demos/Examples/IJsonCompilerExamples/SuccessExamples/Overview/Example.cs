using JsonQL.Compilation;

namespace JsonQL.Demos.Examples.IJsonCompilerExamples.SuccessExamples.Overview;

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
        var additionalTestData = new JsonTextData(
            "AdditionalTestData",
            this.LoadExampleJsonFile("AdditionalTestData.json"));

        var countriesJsonTextData = new JsonTextData(
            "Countries",
            this.LoadExampleJsonFile("Countries.json"), additionalTestData);

        var companiesJsonTextData = new JsonTextData("Companies",
            this.LoadExampleJsonFile("Companies.json"), countriesJsonTextData);

        var result = _jsonCompiler.Compile(new JsonTextData("Overview",
            this.LoadExampleJsonFile("Overview.json"), companiesJsonTextData));
        return result;
    }
}