using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.JsonObjects;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsJsonStructure.Examples.QueryJsonArray;


public class Example : QueryJsonValueExampleManagerForSuccessAbstr
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        string[] sharedExamplesFolderPath = ["DocFiles", "QueryingJsonFiles", "JsonFiles"];

        var parametersJsonTextData = new JsonTextData("Parameters",
            LoadJsonFileHelpers.LoadJsonFile("Parameters.json", sharedExamplesFolderPath));

        var countriesJsonTextData = new JsonTextData("Countries",
            LoadJsonFileHelpers.LoadJsonFile("Countries.json", sharedExamplesFolderPath), parametersJsonTextData);

        var companiesJsonTextData = new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath), countriesJsonTextData);

        var filteredCompaniesJsonTextData = new JsonTextData("FilteredCompanies",
            LoadJsonFileHelpers.LoadJsonFile("FilteredCompanies.json", sharedExamplesFolderPath), companiesJsonTextData);
       
        var query = "FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith'))";

   
        var employeesResult =
            _queryManager.QueryJsonValue(query, filteredCompaniesJsonTextData);

        Assert.That(employeesResult, Is.Not.Null);
        Assert.That(employeesResult.CompilationErrors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.ParsedValue, Is.Not.Null);
        Assert.That(employeesResult.ParsedValue, Is.InstanceOf<IParsedArrayValue>());
        Assert.That(((IParsedArrayValue)employeesResult.ParsedValue!).Values.Count, Is.EqualTo(10));
        return employeesResult;
    }
}