using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.JsonPropertyFormat.Example;

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
        // Select the employees older than 40
        var query = "Employees.Where(e => e.age > 40)";

        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Employees",
                    this.LoadExampleJsonFile("Employees.json")),
                jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
                {
                    JsonPropertyFormat = JsonToObjectConversion.JsonPropertyFormat.CamelCase
                });

        Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.Value, Is.Not.Null);
        Assert.That(employeesResult.Value!.Count, Is.EqualTo(2));
        return employeesResult;
    }
}
