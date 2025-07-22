using JsonQL.Compilation;
using JsonQL.JsonObjects;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsParsedJsonValue.FailedAssert;

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
        // This query will succeed
        var query = "TestData[0] + (TestData[1] assert)";

        var queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")));

        Assert.That(queryResult.ParsedValue is IParsedSimpleValue { Value: "4" });

        // This query will fail since TestData has only 3 items
        query = "TestData[0] + (TestData[1000] assert)";
        
        queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")));

        return queryResult;
    }
}
