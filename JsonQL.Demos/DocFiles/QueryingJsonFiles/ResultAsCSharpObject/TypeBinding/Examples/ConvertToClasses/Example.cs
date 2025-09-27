using JsonQL.Compilation;
using JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses.DataModels;
using JsonQL.Demos.Examples;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ConvertToClasses;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IReadOnlyList<Employee>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<Employee>> QueryObject()
    {
        var query = "Companies.Select(c => c.Employees.Where(e => e.Salary >= 100000))";
        
        // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<Employee>>".
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<Employee>>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", 
                        ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));

        return employeesResult;
    }
}
