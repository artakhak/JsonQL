using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.NullableValueSupport.NullableSyntaxInQueryResultType.Examples.ResultAsValueType;

public class Example : QueryObjectExampleManagerForSuccessAbstr<double>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<double> QueryObject()
    {
        // Select average salary of all employees across all companies
        var query =
             "Average(Companies.Select(c => c.Employees.Select(e => e.Salary)))";

        // The result "averageSalaryResult" is a non-nullable double value according to value
        // used for parameter "convertedValueNullability"
        var averageSalaryResult =
            _queryManager.QueryObject<double>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])),
                convertedValueNullability: [
                    // The result of type "double" cannot be null. An error will be reported if the value is null.
                    false]);

        return averageSalaryResult;
    }
}
