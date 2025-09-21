using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonObjects;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.TypeBinding.Examples.ReplaceInterfaceImplementation;

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
        var query = "Employees.Where(e => e.Salary >= 100000)";
        
        // The result "employeesResult" is of type "JsonQL.Query.IObjectQueryResult<IReadOnlyList<IEmployee>>".
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Employees",
                    this.LoadExampleJsonFile("Employees.json")),
                convertedValueNullability: [false, false],
                jsonConversionSettingOverrides:
                new JsonConversionSettingsOverrides
                {
                    TryMapJsonConversionType = (type, parsedJson) =>
                    {
                        // If we always return null, or just do not set the value, of TryMapJsonConversionType
                        // IEmployee will always be bound to Employee
                        // In this example, we ensure that if parsed JSON has "Employees" as a non-empty array,
                        // then the default implementation of IManager (i.e., Manager) is used to
                        // deserialize the JSON.
                        // We can also specify Manager explicitly.
                        if (//parsedJson.HasKey(nameof(IManager.Employees))
                            parsedJson.TryGetJsonKeyValue(nameof(IManager.Employees), out var employees) &&
                            employees.Value is IParsedArrayValue employeesArray && employeesArray.Values.Count > 0)
                            return typeof(IManager);

                        return null;
                    }
                });

        Assert.That(employeesResult.Value, Is.Not.Null);
        Assert.That(employeesResult.Value!.Count, Is.EqualTo(2));
        Assert.That(employeesResult.Value[0], Is.Not.InstanceOf<IManager>());
        Assert.That(employeesResult.Value[1], Is.InstanceOf<IManager>());
        Assert.That(employeesResult.Value[1], Is.TypeOf<Manager>());

        return employeesResult;
    }
}
