using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsObject.SalariesOfAllEmployeesInAllCompaniesAsArrayOfDoubles;

public class Example : QueryObjectExampleManagerForSuccessAbstr<double[]>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<double[]> QueryObject()
    {
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        var salariesResult =
            _queryManager.QueryObject<double[]>(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")), null);

        return salariesResult;
    }
}
