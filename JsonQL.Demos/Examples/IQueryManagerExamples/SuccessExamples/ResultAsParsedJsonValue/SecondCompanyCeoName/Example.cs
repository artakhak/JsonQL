
using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsParsedJsonValue.SecondCompanyCeoName;

public class Example : QueryJsonValueExampleManagerAbstr
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var query = "Companies[1].CompanyData.CEO";

        var averageSalary =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Example",
                    this.LoadExampleJsonFile("Data.json")));

        return averageSalary;
    }
}