using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.Query;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.Examples.QueryValueTypeValue;


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
        string[] sharedExamplesFolderPath = ["DocFiles", "QueryingJsonFiles", "JsonFiles"];

        var query =
            "Average(Companies.Select(c => c.Employees.Where(e => e.Name != 'John Smith').Select(e => e.Salary)))";
        
        var averageSalaryResult =
            _queryManager.QueryObject<double>(query, new JsonTextData("Companies",
                LoadJsonFileHelpers.LoadJsonFile("Companies.json", sharedExamplesFolderPath)),
                convertedValueNullability: [false]);

        // LogHelper.Context.Log.InfoFormat("Average salary is {0}", averageSalaryResult.Value);

        return averageSalaryResult;
    }
}