using JsonQL.Compilation;
using JsonQL.Demos.Examples;

namespace JsonQL.Demos.DocFiles.JsonPathFunctions.Summary.Example;

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
        var sharedExamplesFolderPath = new []
        {
            "DocFiles", "MutatingJsonFiles", "Examples"
        };

        var companiesJsonTextData = new JsonTextData("Employees",
            LoadJsonFileHelpers.LoadJsonFile("Employees.json", sharedExamplesFolderPath));
      
        var result = _jsonCompiler.Compile(new JsonTextData("Example",
            this.LoadExampleJsonFile("Example.json"), companiesJsonTextData));

        return result;
    }

    protected override bool SerializeOnlyTheLastParsedFile => true;
}
