
using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsObject.AverageSalaryOfAllEmployeesInAllCompanies;

public class Example : QueryObjectExampleManagerAbstr<double>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<double> QueryObject()
    {
        //TempTest<IReadOnlyList<IEmployee?>>();
        var query = "Average(Companies.Select(x => x.Employees.Select(x => x.Salary)))";

        var averageSalaryResult =
            _queryManager.QueryObject<double>(query,
                new JsonTextData("Example", this.LoadExampleJsonFile("Data.json")), null);

        return averageSalaryResult;
    }
}