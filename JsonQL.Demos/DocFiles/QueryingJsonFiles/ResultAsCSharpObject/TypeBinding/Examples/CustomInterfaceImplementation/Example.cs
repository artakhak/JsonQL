using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.CustomInterfaceImplementation;

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
        var queriedJsonFiles = new JsonTextData("Companies",
            LoadJsonFileHelpers.LoadJsonFile("Companies.json", ["DocFiles", "QueryingJsonFiles", "JsonFiles"]));

        var query = "FilteredCompanies.Select(c => c.Employees.Where(e => e.Name !=  'John Smith'))";
        
        // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee>>".
        // The value employeesResult.Value contains the result of the query and is of type IReadOnlyList<IEmployee>.
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                queriedJsonFiles,
                [false, false], new JsonConversionSettingsOverrides
                {
                    TryMapJsonConversionType = (type, parsedJson) =>
                    {
                        // If we always return null, or just do not set the value, of TryMapJsonConversionType
                        // IEmployee will always be bound to Employee
                        // In this example, we ensure that if parsed JSON has "Employees" field,
                        // then the default implementation of IManager (i.e., Manager) is used to
                        // deserialize the JSON.
                        // We can also specify Manager explicitly.
                        if (parsedJson.HasKey(nameof(IManager.Employees)))
                            return typeof(IManager);

                        return null;
                    }
                });

        return employeesResult;
    }
}