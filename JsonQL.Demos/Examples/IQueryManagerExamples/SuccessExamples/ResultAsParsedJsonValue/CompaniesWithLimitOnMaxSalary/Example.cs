
using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsParsedJsonValue.CompaniesWithLimitOnMaxSalary;

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
        var query = "Companies.Where(x => Max(x.Employees, value-> y => y.Salary) < 106000)";

        var averageSalary =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Example",
                    this.LoadExampleJsonFile("Data.json")));

        return averageSalary;
    }
}