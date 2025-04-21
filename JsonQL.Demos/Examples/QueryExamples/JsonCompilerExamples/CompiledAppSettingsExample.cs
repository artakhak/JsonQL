using JsonQL.Compilation;

namespace JsonQL.Demos.Examples.QueryExamples.JsonCompilerExamples;

public class CompiledAppSettingsExample: JsonCompilerExampleManagerAbstr
{
    private readonly IJsonCompiler _jsonCompiler;

    public CompiledAppSettingsExample(IJsonCompiler jsonCompiler)
    {
        _jsonCompiler = jsonCompiler;
    }

    /// <inheritdoc />
    protected override ICompilationResult Compile()
    {
        var globalsSettingsJsonTextData = new JsonTextData("GlobalSettings",
            LoadJsonFileHelpers.LoadJsonFile("GlobalSettings.json",
                ["Examples", "SharedDemoJsonFiles"]));

        var myServiceSettingsJsonTextData = new JsonTextData("MyServiceSettings",
            LoadJsonFileHelpers.LoadJsonFile("MyServiceSettings.json",
                ["Examples", "SharedDemoJsonFiles"]), globalsSettingsJsonTextData);
        
        var result = _jsonCompiler.Compile(myServiceSettingsJsonTextData);
        return result;
    }
}