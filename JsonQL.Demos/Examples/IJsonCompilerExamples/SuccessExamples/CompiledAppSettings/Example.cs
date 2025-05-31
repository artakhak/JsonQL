using JsonQL.Compilation;

namespace JsonQL.Demos.Examples.IJsonCompilerExamples.SuccessExamples.CompiledAppSettings;

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
        var globalsSettingsJsonTextData = new JsonTextData(
            "GlobalSettings",
            this.LoadExampleJsonFile("GlobalSettings.json"));

        var myServiceSettingsJsonTextData = new JsonTextData("MyServiceSettings",
            this.LoadExampleJsonFile("MyServiceSettings.json"), globalsSettingsJsonTextData);

        var result = _jsonCompiler.Compile(myServiceSettingsJsonTextData);
        return result;
    }
}