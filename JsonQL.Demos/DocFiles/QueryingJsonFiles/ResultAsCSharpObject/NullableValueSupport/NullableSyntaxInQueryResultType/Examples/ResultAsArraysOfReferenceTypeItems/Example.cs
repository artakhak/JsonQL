using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.NullableValueSupport.NullableSyntaxInQueryResultType.Examples.ResultAsArraysOfReferenceTypeItems;

public class Example : QueryObjectExampleManagerForSuccessAbstr<IEmployee?[]>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IEmployee?[]> QueryObject()
    {
        var query = "Employees.Where(e => e.Id != 250150245)";

        // The result "employeesResult" is a nullable list of nullable IEmployee values. 
        // Result of type "IEmployee?[]" cannot be null, and each IEmployee
        // array "IEmployee?[]" can be null according to value used for parameter
        // "convertedValueNullability"
        var employeesResult =
            _queryManager.QueryObject<IEmployee?[]>(query,
                new JsonTextData("Employees",
                    this.LoadExampleJsonFile("Employees.json")),
                convertedValueNullability: [
                    // The result of type "IEmployee?[]" cannot be null. An error will be reported if the value is null
                    false,
                    // "IEmployee" items in list "IEmployee?[]" can be null.
                    true]);

        return employeesResult;
    }
}
