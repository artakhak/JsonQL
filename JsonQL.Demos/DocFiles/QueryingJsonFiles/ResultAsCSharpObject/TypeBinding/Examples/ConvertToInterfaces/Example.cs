using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;
using Employee = JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels.Employee;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToInterfaces;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IReadOnlyList<IEmployee>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<IEmployee>> QueryObject()
    {
        var query = "Companies.Select(c => c.Employees.Where(e => e.Salary >= 100000))";
        
        // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee>>".
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", 
                        ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));

        return employeesResult;
    }
}
