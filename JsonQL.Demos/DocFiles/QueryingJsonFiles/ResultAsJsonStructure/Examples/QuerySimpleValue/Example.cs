using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Query;

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

        return averageSalaryResult;
    }
}