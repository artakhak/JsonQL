using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsObject.SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IReadOnlyList<double>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<double>> QueryObject()
    {
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        var salariesResult =
            _queryManager.QueryObject<IReadOnlyList<double>>(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")), null);

        return salariesResult;
    }
}
