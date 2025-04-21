using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsParsedJsonValue;

public class GetAllEmployeeSalaries : QueryJsonValueExampleManagerAbstr
{
    private readonly IQueryManager _queryManager;

    public GetAllEmployeeSalaries(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";
        var averageSalary =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["Examples", "SharedDemoJsonFiles"])));

        return averageSalary;
    }
}