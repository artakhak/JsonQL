
using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsParsedJsonValue.SalariesOfAllEmployeesInAllCompanies;

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
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";
        
        var averageSalary =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Example",
                    this.LoadExampleJsonFile("Data.json")));

        return averageSalary;
    }
}