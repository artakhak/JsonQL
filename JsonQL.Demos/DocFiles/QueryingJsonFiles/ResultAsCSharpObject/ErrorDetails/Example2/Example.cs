using JsonQL.Compilation;
using JsonQL.Demos.Examples;
using JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.DocFiles.QueryingJsonFiles.ResultAsCSharpObject.ErrorDetails.Example2;

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
        // This query will fail since not all values of IEmployee.Age are non-null in a result set.
        var query = "Employees.Where(x => x.Salary >= 100000)";
        
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Employees", LoadJsonFileHelpers.LoadJsonFile("Employees.json",
                    ["DocFiles", "QueryingJsonFiles", "ResultAsCSharpObject", "ErrorDetails"])), 
                convertedValueNullability:null,
                jsonConversionSettingOverrides:
                // NOTE: jsonConversionSettingOverrides parameter of type IJsonConversionSettingsOverrides
                // is an optional, and we do not have to provide this parameter of default settings work for us (which is most of the cases).
                // However, the parameter is specified here as an example
                new JsonConversionSettingsOverrides
                {
                    ConversionErrorTypeConfigurations = [
                        // Note, we only need to provide configurations that we want to override.
                        // Default configurations will be used for any error type that is not specified in 
                        // ConversionErrorTypeConfigurations collection
                        new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, 
                            // The default error reporting type of ConversionErrorType.NonNullablePropertyNotSet is
                            // ErrorReportingType.ReportAsError.
                            // This is just a demo how the default configuration can be overridden
                            ErrorReportingType.ReportAsError)] 
                });

        Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.GreaterThan(0));
        return employeesResult;
    }
}
