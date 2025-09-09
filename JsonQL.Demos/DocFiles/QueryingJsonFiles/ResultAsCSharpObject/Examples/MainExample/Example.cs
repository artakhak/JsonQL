using JsonQL.Compilation;
using JsonQL.Demos.Examples;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.Examples.MainExample;

public class Example : JsonCompilerExampleManagerForSuccessAbstr
{
    private readonly IJsonCompiler _jsonCompiler;

    private static readonly string[] _sharedExamplesFolderPath = new string[] {
        "DocFiles", "MutatingJsonFiles", "Examples"
    };

    public Example(IJsonCompiler jsonCompiler)
    {
        _jsonCompiler = jsonCompiler;
    }

    /// <inheritdoc />
    protected override ICompilationResult Compile()
    {
        var parametersJsonTextData = new JsonTextData("Parameters",
            LoadJsonFileHelpers.LoadJsonFile("Parameters.json", _sharedExamplesFolderPath));

        var countriesJsonTextData = new JsonTextData("Countries",
            LoadJsonFileHelpers.LoadJsonFile("Countries.json", _sharedExamplesFolderPath), parametersJsonTextData);

        var companiesJsonTextData = new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", _sharedExamplesFolderPath), countriesJsonTextData);

        var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
            LoadJsonFileHelpers.LoadJsonFile("FilteredCompanies.json",  _sharedExamplesFolderPath), companiesJsonTextData);
      
        var result = _jsonCompiler.Compile(new JsonTextData("Example",
            this.LoadExampleJsonFile("Example.json"), filteredCompaniesJsonTextData));
        return result;
    }

    protected override bool SerializeOnlyTheLastParsedFile => false;
}
