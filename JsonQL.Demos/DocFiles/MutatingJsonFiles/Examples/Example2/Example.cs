using JsonQL.Compilation;
using JsonQL.Demos.Examples;

namespace JsonQL.Demos.DocFiles.MutatingJsonFiles.Examples.Example2;

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
            this.LoadExampleJsonFile("Parameters.json"));

        var countriesJsonTextData = new JsonTextData("Countries",
            LoadJsonFileHelpers.LoadJsonFile("Countries.json", _sharedExamplesFolderPath), parametersJsonTextData);

        var companiesJsonTextData = new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", _sharedExamplesFolderPath), countriesJsonTextData);

        var cachedCompilationResult = _jsonCompiler.Compile(new JsonTextData("FilteredCompanies",
            this.LoadExampleJsonFile("FilteredCompanies.json"), companiesJsonTextData));

        if (cachedCompilationResult.CompilationErrors.Count > 0)
            throw new ApplicationException("Compilation failed");
        
        var compiledParents = new List<ICompiledJsonData>
        {
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Companies")
        };

        var jsonThatDependsOnCompanies = 
            string.Concat("{\"AllCompanyNames:\": \"$value(Companies.Select(x => x.CompanyData.Name))\"," +
            "\"AllCompanyEmployees:\": \"$value(Companies.Where(x => !(x.CompanyData.Name starts with 'Strange')).Select(x => x.Employees))\"}");
        
        var jsonThatDependsOnCompaniesResult = _jsonCompiler.Compile(jsonThatDependsOnCompanies, "Json1", compiledParents);
        // Do something with jsonThatDependsOnCompaniesResult here.

        compiledParents = new List<ICompiledJsonData>
        {
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Parameters"),
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Countries"),
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Companies"),
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "FilteredCompanies"),
        };

        var exampleJsonResult = _jsonCompiler.Compile(this.LoadExampleJsonFile("Example.json"), "Example", compiledParents);
        return exampleJsonResult;
    }

    protected override bool SerializeOnlyTheLastParsedFile => false;
}
