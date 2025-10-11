using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonObjects;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.TryMapJsonConversionType.Example;

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
        // MAKE SURE TURN_ON_DOCUMENTATION_TEST_SETTINGS IS TURNED IN JsonQL PROJECT BEFORE RUNNING THIS TEST!!!!!!

        // Select all employees
        var query = "Employees";
        
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("EmployeesWithMissingData",
                    LoadJsonFileHelpers.LoadJsonFile("Employees.json", 
                        ["DocFiles", "QueryingJsonFiles", "ResultAsCSharpObject", 
                            "ConversionSettings", "TryMapJsonConversionType", "Example"])),
                jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
                {
                    TryMapJsonConversionType =
                        (defaultTypeToConvertParsedJsonTo, convertedParsedJson) =>
                        {
                            if (defaultTypeToConvertParsedJsonTo == typeof(IEmployee))
                            {
                                // The delegate JsonQL.JsonToObjectConversion.IJsonConversionSettings.TryMapJsonConversionType
                                // will use IManager if "Employees" is present. Lets return IEmployees if JSON
                                // array Employees is either null or is empty.
                                if (convertedParsedJson.HasKey(nameof(IManager.Employees)) &&
                                    !(convertedParsedJson.TryGetJsonKeyValue(nameof(IManager.Employees), out var employeesJson) &&
                                      employeesJson.Value is IParsedArrayValue parsedArrayValue && parsedArrayValue.Values.Count > 0))
                                    return typeof(IEmployee);
                            }
                
                            // Returning null will result in either delegate used for
                            // JsonQL.JsonToObjectConversion.IJsonConversionSettings.TryMapJsonConversionType being used to map the type,
                            // or if the call to JsonQL.JsonToObjectConversion.IJsonConversionSettings.TryMapJsonConversionType returns 
                            // null, the default mapping mechanism picking a type to use.
                            return null;
                        }
                });
        
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.Value, Is.Not.Null);
        Assert.That(employeesResult.Value!.Count, Is.EqualTo(4));
        
        Assert.That(employeesResult.Value[0].GetType() == typeof(Employee));
        Assert.That(employeesResult.Value[1].GetType() == typeof(Manager));
        Assert.That(employeesResult.Value[2].GetType() == typeof(CustomEmployee));
        Assert.That(employeesResult.Value[3].GetType() == typeof(Employee));
        
        return employeesResult;
    }
}
