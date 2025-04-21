using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsObject;

public class QuerySalariesAsIReadOnlyListOfDoubleValues : QueryObjectExampleManagerAbstr<IReadOnlyList<double>>
{
    private readonly IQueryManager _queryManager;

    public QuerySalariesAsIReadOnlyListOfDoubleValues(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<double>> QueryObject()
    {
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        var salariesResult =
            _queryManager.Query<IReadOnlyList<double>>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["Examples", "SharedDemoJsonFiles"])));

        return salariesResult;
    }
}