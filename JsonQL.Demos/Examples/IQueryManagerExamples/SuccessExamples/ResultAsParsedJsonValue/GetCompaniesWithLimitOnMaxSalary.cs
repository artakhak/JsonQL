using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsParsedJsonValue;

public class GetCompaniesWithLimitOnMaxSalary : QueryJsonValueExampleManagerAbstr
{
    private readonly IQueryManager _queryManager;

    public GetCompaniesWithLimitOnMaxSalary(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        var query = "Companies.Where(x => Max(x.Employees, value-> y => y.Salary) < 106000)";
        var companyWithEmployeeWithSalaryLessThan_106000 =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["Examples", "SharedDemoJsonFiles"])));

        return companyWithEmployeeWithSalaryLessThan_106000;
    }
}