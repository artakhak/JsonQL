using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsObject.SalariesOfAllEmployeesInAllCompaniesAsListOfDoubles;

public class Example : QueryObjectExampleManagerForSuccessAbstr<List<double>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<List<double>> QueryObject()
    {
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        var averageSalaryResult =
            _queryManager.QueryObject<List<double>>(query,
                new JsonTextData("Example",
                    this.LoadExampleJsonFile("Data.json")), null);

        return averageSalaryResult;
    }
}