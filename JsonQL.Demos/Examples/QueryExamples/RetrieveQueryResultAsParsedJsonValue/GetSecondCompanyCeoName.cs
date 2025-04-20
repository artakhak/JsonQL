using JsonQL.Compilation;
using JsonQL.Extensions.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsParsedJsonValue;

public class GetSecondCompanyCeoName : QueryJsonValueExampleManagerAbstr
{
    private readonly IQueryManager _queryManager;

    public GetSecondCompanyCeoName(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var query = "Companies[1].CompanyData.CEO";
        var secondCompanyOfRobinWood =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Companies", LoadJsonFileHelpers.LoadJsonFile("Companies.json",
                    ["Examples", "SharedDemoJsonFiles"])));

        return secondCompanyOfRobinWood;
    }
}