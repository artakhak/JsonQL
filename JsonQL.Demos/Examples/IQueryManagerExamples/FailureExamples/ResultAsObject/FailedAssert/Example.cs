using JsonQL.Compilation;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.FailedAssert;

public class Example : QueryObjectExampleManagerForFailureAbstr<double>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<double> QueryObject()
    {
        // This query will succeed
        /*var query = "TestData[0] + (TestData[1] assert)";
        var resultAsDouble =
            _queryManager.QueryObject<double>(query,
                new JsonTextData("Example", this.LoadExampleJsonFile("Data.json")), null);

        Assert.That(resultAsDouble.Value, Is.EqualTo(4));*/

        // This query will fail since TestData has only 3 items
        var query = "TestData[0] + (TestData[1000] assert)";

        var resultAsDouble =
            _queryManager.QueryObject<double>(query,
                new JsonTextData("Example", this.LoadExampleJsonFile("Data.json")), null);

        return resultAsDouble;
    }
}