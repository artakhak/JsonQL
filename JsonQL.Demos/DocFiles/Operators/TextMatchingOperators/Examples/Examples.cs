using JsonQL.Compilation;
using JsonQL.Demos.Examples;

namespace JsonQL.Demos.DocFiles.Operators.TextMatchingOperators.Examples;

public class Examples : JsonCompilerExampleManagerForSuccessAbstr
{
    private readonly IJsonCompiler _jsonCompiler;

    public Examples(IJsonCompiler jsonCompiler)
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

        var dataJsonTextData = new JsonTextData("Data", this.LoadExampleJsonFile("Data.json"));
        
        var companiesJsonTextData = new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), dataJsonTextData);
        
        var result = _jsonCompiler.Compile(new JsonTextData("Examples",
            this.LoadExampleJsonFile("Examples.json"), companiesJsonTextData));
        
        return result;
    }

    protected override bool SerializeOnlyTheLastParsedFile => true;
}
