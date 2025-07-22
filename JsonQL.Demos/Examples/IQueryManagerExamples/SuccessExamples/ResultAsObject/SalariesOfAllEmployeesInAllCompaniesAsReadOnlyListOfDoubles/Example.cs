using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.SuccessExamples.ResultAsObject.SalariesOfAllEmployeesInAllCompaniesAsReadOnlyListOfDoubles;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IReadOnlyList<double>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<double>> QueryObject()
    {
        var salariesOfAllEmployeesOlderThan35InAllCompaniesQuery = 
            "Companies.Select(x => x.Employees.Where(x => x.Age > 35).Select(x => x.Salary))";

        var salariesResult =
            _queryManager.QueryObject<IReadOnlyList<double>>(salariesOfAllEmployeesOlderThan35InAllCompaniesQuery,
                new JsonTextData("Data",
                    this.LoadExampleJsonFile("Data.json")), null);

        return salariesResult;
    }
}
