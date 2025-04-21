using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsParsedJsonValue;

public class GetSecondCompany : QueryJsonValueExampleManagerAbstr
{
    private readonly IQueryManager _queryManager;

    public GetSecondCompany(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var query = "Companies[1]";

        var secondCompany =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Companies", LoadJsonFileHelpers.LoadJsonFile("Companies.json",
                    ["Examples", "SharedDemoJsonFiles"])));

        return secondCompany;
    }
}