using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsJsonStructure.ErrorDetails.Example1;

public class Example : QueryJsonValueExampleManagerForSuccessAbstr
{
    private readonly IQueryManager _queryManager;

    public Example(IQueryManager queryManager)
    {
        _queryManager = queryManager;
    }

    /// <inheritdoc />
    protected override IJsonValueQueryResult QueryJsonValue()
    {
        // This query will result in compilation error since closing brace is missing for the open brace.
        var query = "Companies.Select(c => c.Employees.Where(e => e.Address is null)";
        
        var queryResult =
            _queryManager.QueryJsonValue(query,
                new JsonTextData("Companies", 
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"])));

        Assert.That(queryResult.CompilationErrors.Count, Is.EqualTo(1));
        return queryResult;
    }
}
