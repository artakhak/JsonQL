using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsObject;

public class QuerySalariesAsArrayOfDoubleValues : QueryObjectExampleManagerAbstr<double[]>
{
    private readonly IQueryManager _queryManager;

    public QuerySalariesAsArrayOfDoubleValues(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<double[]> QueryObject()
    {
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        var averageSalaryResult =
            _queryManager.QueryObject<double[]>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["Examples", "SharedDemoJsonFiles"])), null);

        return averageSalaryResult;
    }
}