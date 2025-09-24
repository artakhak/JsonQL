using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonObjects;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsJsonStructure.Examples.QueryJsonObject;


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
        
        var query = "FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith')).First(x => x.Age >= 40)";
        
        var employeeResult =
            _queryManager.QueryJsonValue(query, filteredCompaniesJsonTextData);
        
        Assert.That(employeeResult, Is.Not.Null);
        Assert.That(employeeResult.CompilationErrors.Count, Is.EqualTo(0));
        Assert.That(employeeResult.ParsedValue, Is.Not.Null);
        
        // We don't care about the null / conversion exception in this example for brevity.
        var parsedJson = (IParsedJson)employeeResult.ParsedValue!;
        
        Assert.That(((IParsedSimpleValue)parsedJson["Id"].Value).Value, Is.EqualTo("100000001"));
        
        return employeeResult;
    }
}