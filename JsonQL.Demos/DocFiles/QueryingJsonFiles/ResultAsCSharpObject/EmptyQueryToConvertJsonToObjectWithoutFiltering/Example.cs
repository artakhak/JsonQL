using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.EmptyQueryToConvertJsonToObjectWithoutFiltering;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IEmployee>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IEmployee> QueryObject()
    {
        var query = string.Empty;

        // The result "employeesResult" is a non-nullable IEmployee value.
        var employeeResult =
            _queryManager.QueryObject<IEmployee>(query,
                new JsonTextData("Employee",
                    this.LoadExampleJsonFile("Employee.json"),
                    new JsonTextData("Companies",
                        LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"]))),
                convertedValueNullability: [
                    // The result of type "IEmployee" cannot be null. An error will be reported if the value is null
                    false]);

        return employeeResult;
    }
}
