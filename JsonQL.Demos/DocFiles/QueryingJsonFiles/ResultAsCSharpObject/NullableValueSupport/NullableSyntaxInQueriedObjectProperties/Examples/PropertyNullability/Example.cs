using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.NullableValueSupport.NullableSyntaxInQueriedObjectProperties.Examples.PropertyNullability;

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
        // Select the employees in all companies with non-null value for Address
        var query = 
            "Companies.Select(c => c.Employees).Where(e => e.Address is not null)";

        // Employees in employeesResult.Value of type IReadOnlyList<IEmployee> can have null for 
        // property IEmployee.Manager since this property is of type "IManager?" (uses nullable syntax)
        // Also, the values of IEmployee.Address.County can be null in result too
        // since the property IAddress.County is of type "string?" (nullable string)
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));
        
        return employeesResult;
    }
}
