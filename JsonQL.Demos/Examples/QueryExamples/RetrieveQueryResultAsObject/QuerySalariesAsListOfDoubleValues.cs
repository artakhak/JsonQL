using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsObject;

public class QuerySalariesAsListOfDoubleValues : QueryObjectExampleManagerAbstr<List<double>>
{
    private readonly IQueryManager _queryManager;

    public QuerySalariesAsListOfDoubleValues(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }
    
    /// <inheritdoc />
    protected override IObjectQueryResult<List<double>> QueryObject()
    {
        var query = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        var salariesResult =
            _queryManager.Query<List<double>>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["Examples", "SharedDemoJsonFiles"])));

        return salariesResult;
    }
}