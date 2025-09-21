using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ErrorDetails.Example1;

public class Example : QueryObjectExampleManagerForFailureAbstr<IReadOnlyList<IEmployee>>
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IObjectQueryResult<IReadOnlyList<IEmployee>> QueryObject()
    {
        // This query will result in compilation error since closing brace is missing for the open brace.
        var query = "Companies.Select(c => c.Employees.Where(e => e.Address is null)";
        
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Companies", 
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));

        Assert.That(employeesResult.ErrorsAndWarnings.CompilationErrors.Count, Is.GreaterThan(0));
        return employeesResult;
    }
}
