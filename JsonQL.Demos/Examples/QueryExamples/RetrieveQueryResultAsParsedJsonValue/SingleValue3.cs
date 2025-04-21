using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.QueryExamples.RetrieveQueryResultAsParsedJsonValue;

public class SingleValue3 : QueryJsonValueExampleManagerAbstr
{
    private readonly IQueryManager _queryManager;

    public SingleValue3(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }
    
    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var query = "Companies.Where(x => x.CompanyData.CEO == 'Robin Wood')";

        var companyOfRobinWood =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["Examples", "SharedDemoJsonFiles"])));

        return companyOfRobinWood;
    }
}