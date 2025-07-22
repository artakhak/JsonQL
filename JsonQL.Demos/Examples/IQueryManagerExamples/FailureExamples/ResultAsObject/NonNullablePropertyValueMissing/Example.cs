using JsonQL.Compilation;
using JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing.DataModels;
using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using NUnit.Framework;

namespace JsonQL.Demos.Examples.IQueryManagerExamples.FailureExamples.ResultAsObject.NonNullablePropertyValueMissing;

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
        FilterOutEmployeesWithNullValueOfAddressWithoutNullabilityViolationErrors();
        LoadAllEmployeesWithTurningOffNullabilityCheck();

        // This query will fail since not all values of IEmployee.Age are non-null in result set.
        var query = "Employees";
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Employees", this.LoadExampleJsonFile("Employees.json")), null,

                // NOTE: jsonConversionSettingOverrides parameter of type IJsonConversionSettingsOverrides
                // is an optional, and we do not have to provide this parameter of default settings work for us (which is most of the cases).
                // However, the parameter is specified here as an example
                new JsonConversionSettingsOverrides
                {
                    ConversionErrorTypeConfigurations = [
                        // Note, we only need to provide configurations that we want to override.
                        // Default configurations will be used for any error type that is not specified in 
                        // ConversionErrorTypeConfigurations collection
                        new ConversionErrorTypeConfiguration(ConversionErrorType.FailedToConvertJsonValueToExpectedType, 
                            // The default error reporting type of ConversionErrorType.FailedToConvertJsonValueToExpectedType is
                            // ErrorReportingType.ReportAsError.
                            // This is just a demo how the default configuration can be overridden
                            ErrorReportingType.ReportAsWarning)] 
                });
        
        return employeesResult;
    }

    /// <summary>
    /// Successfully load employees, by filtering out employees with null values of non-nullable <see cref="IEmployee.Address"/> property.
    /// </summary>
    private void FilterOutEmployeesWithNullValueOfAddressWithoutNullabilityViolationErrors()
    {
        // This query will succeed, because we skip employee with Id=100000002 that has null value for IEmployee.Address property
        var query = "Employees.Where(x => x.Id != 100000002)";
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Employees", this.LoadExampleJsonFile("Employees.json")), null);
        
        Assert.That(employeesResult.Value?.Count, Is.EqualTo(3));
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.ErrorsAndWarnings.CompilationErrors.Count, Is.EqualTo(0));
    }

    /// <summary>
    /// Successfully loads all employees, including employees with null values of non-nullable <see cref="IEmployee.Address"/> property,
    /// by turning off nullability check.
    /// </summary>
    private void LoadAllEmployeesWithTurningOffNullabilityCheck()
    {
        // This query will succeed, because we skip employee with Id=100000002 that has null value for IEmployee.Address property
        var query = "Employees";
        var employeesResult =
            _queryManager.QueryObject<IReadOnlyList<IEmployee>>(query,
                new JsonTextData("Employees", this.LoadExampleJsonFile("Employees.json")), null, new JsonConversionSettingsOverrides
                {
                    JsonPropertyFormat = JsonPropertyFormat.PascalCase,
                    ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                    {
                        new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.Ignore),
                        new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, ErrorReportingType.ReportAsError),
                        new ConversionErrorTypeConfiguration(ConversionErrorType.CannotCreateInstanceOfClass, ErrorReportingType.ReportAsError)
                    }
                });

        Assert.That(employeesResult.Value?.Count, Is.EqualTo(4));
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.ErrorsAndWarnings.ConversionWarnings.Errors.Count, Is.EqualTo(0));
        Assert.That(employeesResult.ErrorsAndWarnings.CompilationErrors.Count, Is.EqualTo(0));
    }
}
