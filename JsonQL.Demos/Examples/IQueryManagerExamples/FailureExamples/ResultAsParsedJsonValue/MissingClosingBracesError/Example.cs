using JsonQL.Compilation;
using JsonQL.JsonObjects;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsParsedJsonValue.MissingClosingBracesError;

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
        var query = "ToInt(TestData[3]) + 1";

        // This query will succeed.
        var queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Example",
                    this.LoadExampleJsonFile("Data.json")));

        Assert.That(queryResult.ParsedValue is IParsedSimpleValue { Value: "6" });

        // This query will fail since there is no closing brace for function ToInt.
        query = "ToInt(TestData[3] + 1";

        queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Example",
                    this.LoadExampleJsonFile("Data.json")));

        return queryResult;
    }
}