using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsParsedJsonValue.SecondCompany;

public class Example : QueryJsonValueExampleManagerForSuccessAbstr
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var query = "Companies[1]";

        var secondCompanyResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")));

        return secondCompanyResult;
    }
}
