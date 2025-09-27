using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.JsonObjects;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsJsonStructure.Examples.QuerySimpleValue;


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
        
        var query =
            "Average(Companies.Select(c => c.Employees.Where(e => e.Name != 'John Smith').Select(e => e.Salary)))";
        
        var averageSalaryResult =
            _queryManager.QueryJsonValue(query, new JsonTextData("Companies",
                LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath)));
        
        Assert.That(averageSalaryResult, Is.Not.Null);
        Assert.That(averageSalaryResult.CompilationErrors.Count, Is.EqualTo(0));
        Assert.That(averageSalaryResult.ParsedValue, Is.Not.Null);
        
        // We don't care about the null / conversion exception in this example for brevity.
        var parsedSimpleValue = (IParsedSimpleValue)averageSalaryResult.ParsedValue!;
        
        Assert.That(parsedSimpleValue.IsString, Is.False);
        Assert.That(parsedSimpleValue.Value, Is.EqualTo("102356.75"));
        
        return averageSalaryResult;
    }
}
