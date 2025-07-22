using JsonQL.Compilation;
using JsonQL.JsonObjects;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsParsedJsonValue.InvalidParametersError;

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
        var query = "Lower(TestData[3])";

        // This query will succeed.
        var queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")));

        Assert.That(queryResult.ParsedValue is IParsedSimpleValue {Value: "test"});

        // This query will fail as it has extra parameter.
        query = "Lower(TestData[3], 7)";

        queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")));

        return queryResult;
    }
}
