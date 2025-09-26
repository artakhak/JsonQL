using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonObjects;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ReusingCompiledJsonFiles.Example;

public class Example : QueryJsonValueExampleManagerForSuccessAbstr
{
    private readonly IJsonCompiler _jsonCompiler;
    private readonly IQueryManager _queryManager;

    public Example(IJsonCompiler jsonCompiler, IQueryManager queryManager)
    {
        _jsonCompiler = jsonCompiler;
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var sharedExamplesFolderPath = new []
        {
            "DocFiles", "QueryingJsonFiles", "JsonFiles"
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

        // The first query (see employeesResult below) needs only compiled and cached "FilteredCompanies" file.
        // So lets get this compiled file from cachedCompilationResult and store it in "compiledParents" list
        // that we will pass to JsonQL.Query.IQueryManager.QueryObject<IReadOnlyList<IEmployee>>(query, compiledParents) below
        var compiledParents = new List<ICompiledJsonData>
        {
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "FilteredCompanies")
        };

        var employeesResult = _queryManager.QueryObject<IReadOnlyList<IEmployee>>(
            "FilteredCompanies.Select(c => c.Employees).Where(e => e.Age >= 40)",
            compiledParents);

        Assert.That(employeesResult.Value is not null && employeesResult.Value.Count == 6);

        // The second query below is executed using JsonQL.Query.IQueryManager.QueryJsonValue(query, parents).
        // The query executes against the files "Countries" and "Parameters". 
        // So lets get these compiled files from cachedCompilationResult and store them in "compiledParents" list
        // that we will pass to JsonQL.Query.IQueryManager.QueryJsonValue(query, parents).
        // NOTE: The list of parents compiledParents passed to JsonQL.Query.IQueryManager.QueryJsonValue(...)
        // is organized in such a way that child JSON files appear earlier, and parent JSON files appear later.
        // In this example "Countries.json" will be treated as a child of "Parameters".
        // This relationship will ensure that JSON objects referenced in JsonQL expressions will be looked up
        // first in "Countries.json" and then in "Countries".
        compiledParents =
        [
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Countries"),
            cachedCompilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == "Parameters")
        ];

        var countriesResult = _queryManager.QueryJsonValue("Countries.Where(c => Any(FilteredCountryNames.Where(fc => fc == c.Name)))", compiledParents);

        Assert.That(countriesResult.ParsedValue is not null && 
                    countriesResult.ParsedValue is IParsedArrayValue countriesJsonArray &&
                    countriesJsonArray.Values.Count == 2);

        return countriesResult;
    }

    protected override bool SerializeOnlyTheLastParsedFile => false;
}
