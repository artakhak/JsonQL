using JsonQL.Compilation;
using JsonQL.Demos.Examples;

namespace JsonQL.Demos.DocFiles.MutatingJsonFiles.Examples.ReusingCompiledJsonFilesExample;

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

        var parametersJsonTextData = new JsonTextData("Parameters",
            LoadJsonFileHelpers.LoadJsonFile("Parameters.json", sharedExamplesFolderPath));

        var countriesJsonTextData = new JsonTextData("Countries",
            LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

        var companiesJsonTextData = new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

        var cachedCompilationResult = _jsonCompiler.Compile(new JsonTextData("FilteredCompanies",
            LoadJsonFileHelpers.LoadJsonFile("FilteredCompanies.json", sharedExamplesFolderPath), companiesJsonTextData));

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

        // NOTE: The list of parents compiledParents passed to _jsonCompiler.Compile() is organized in such a way
        // that child JSON files appear earlier, and parent JSON files appear later.
        // In this example "Example.json" will be treated as a child of "FilteredCompanies", "FilteredCompanies" will be treated as a child of
        // "Companies" and so forth. 
        // This relationship will ensure that JSON objects referenced in JsonQL expressions in "Example.json" will
        // be looked up first in "Example.json", then in "FilteredCompanies", and so forth.
        compiledParents =
        [
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "FilteredCompanies"),
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Companies"),
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Countries"),
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Parameters")
        ];

        var exampleJsonResult = _jsonCompiler.Compile(this.LoadExampleJsonFile("Example.json"), "Example", compiledParents);
        return exampleJsonResult;
    }

    protected override bool SerializeOnlyTheLastParsedFile => false;
}
