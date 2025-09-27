using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.ConversionErrorTypeConfigurations.ReportErrorsAsErrors;

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
        // Select the employees with null or missing values for non-null properties
        var query =
            "Employees.Where(e => e.Address is null || e.Address is undefined || e.Phones is null || e.Phones is undefined)";

        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("EmployeesWithMissingData",
                    LoadJsonFileHelpers.LoadJsonFile("EmployeesWithMissingData.json", 
                        ["DocFiles", "QueryingJsonFiles", "ResultAsCSharpObject", 
                            "ConversionSettings", "ConversionErrorTypeConfigurations"])),
                jsonConversionSettingOverrides: null);

        Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(1));
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.Value, Is.Null);
        return employeesResult;
    }
}
