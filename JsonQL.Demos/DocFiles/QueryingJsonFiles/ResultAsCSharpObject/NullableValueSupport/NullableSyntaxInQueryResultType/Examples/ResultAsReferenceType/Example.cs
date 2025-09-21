using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.NullableValueSupport.NullableSyntaxInQueryResultType.Examples.ResultAsReferenceType;

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
        // Select the first employee older than 40 in company with CompanyData.Name=='Atlantic Transfers, Inc'
        var query = 
            "Companies.Where(x => x.CompanyData.Name=='Atlantic Transfers, Inc').Select(c => c.Employees).First(e => e.Age > 40)";

        // The result "employeeResult" is a non-nullable IEmployee value according to value
        // used for parameter "convertedValueNullability"
        var employeeResult =
            _queryManager.QueryObject<IEmployee>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])),
                convertedValueNullability: [
                    // The result of type "IEmployee" cannot be null. An error will be reported if the value is null.
                    false]);

        return employeeResult;
    }
}
