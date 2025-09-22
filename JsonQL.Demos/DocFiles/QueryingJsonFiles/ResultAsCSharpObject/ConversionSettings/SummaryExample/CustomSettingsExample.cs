using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.DataModels;
using JsonQL.JsonObjects;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ConversionSettings.SummaryExample;

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
        // Select the employees in all companies with non-null value for Address
        var query = 
            "Companies.Select(c => c.Employees).Where(e => e.Address is not null)";
        
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Companies",
                    LoadJsonFileHelpers.LoadJsonFile("Companies.json", 
                        ["DocFiles", "QueryingJsonFiles", "JsonFiles"])),
                jsonConversionSettingOverrides: new JsonConversionSettingsOverrides
                {
                    // Lets report nullable values not set errors as warnings. The other errors not specified in 
                    // ConversionErrorTypeConfigurations will use the values set in 
                    // JsonQL.JsonToObjectConversion.IJsonConversionSettings
                    ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                    {
                        new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.ReportAsWarning),
                        new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.ReportAsWarning),
                        new ConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, ErrorReportingType.ReportAsWarning)
                    },
                    FailOnFirstError = false,
                    TryMapJsonConversionType = (defaultTypeToConvertParsedJsonTo, convertedParsedJson) =>
                    {
                        if (defaultTypeToConvertParsedJsonTo == typeof(Employee) &&
                            convertedParsedJson.HasKey(nameof(IManager.Employees)) &&
                            !(convertedParsedJson.TryGetJsonKeyValue(nameof(IManager.Employees), out var employees) &&
                              employees is IParsedArrayValue employeesArray &&
                              employeesArray.Values.Count > 0))
                        {
                            return typeof(ManagerWithoutEmployees);
                        }
                        
                        // Returning null will result in configuration setup in 
                        // JsonQL.JsonToObjectConversion.IJsonConversionSettings being used.
                        return null;
                    }
                });
        
        return employeesResult;
    }
}
