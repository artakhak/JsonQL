using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsJsonStructure.ErrorDetails.Examples.MissingBraceInQueriedFileExample;

public class Example : QueryJsonValueExampleManagerForFailureAbstr
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        // This query will fail since the value of JSON key "JsonQlExpressionWithMissingClosingBraces"
        // is a JsonQL "$value(ToInt(TestData[3]) + 1" that has no closing brace for
        // "$value(" function.
        var query = "JsonQlExpressionWithMissingClosingBraces";
        
        var queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")));
        
        return queryResult;
    }
}
