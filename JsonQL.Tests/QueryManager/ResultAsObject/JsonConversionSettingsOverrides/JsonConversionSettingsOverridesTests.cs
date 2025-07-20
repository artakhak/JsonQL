using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using JsonQL.Tests.QueryManager.ResultAsObject.JsonConversionSettingsOverrides.Models;

namespace JsonQL.Tests.QueryManager.ResultAsObject.JsonConversionSettingsOverrides;

[TestFixture]
public class JsonConversionSettingsOverridesTests : ResultValidatingTestsAbstr
{
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "JsonConversionSettingsOverrides", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "JsonConversionSettingsOverrides", "ExpectedResults"];

    /// <summary>
    /// For more tests/demos for <see cref="IJsonConversionSettingsOverrides.ConversionErrorTypeConfigurations"/> look at
    /// <see cref="JsonQL.Tests.QueryManager.ResultAsObject.ConversionErrors.ConversionErrorsTests"/>.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task ConversionErrorTypeConfigurations_Test()
    {
        var employeesWithNullAddressQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Address is null)))";

        // A query which fails with NonNullablePropertyNotSet errors
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData);
              
                Assert.That(queryResult.Value, Is.Null);

                Assert.That(queryResult.ErrorsAndWarnings.ConversionErrors.Errors.Any(
                    x => x.ErrorType == ConversionErrorType.NonNullablePropertyNotSet), Is.True);
                Assert.That(queryResult.ErrorsAndWarnings.ConversionWarnings.Errors.All(
                    x => x.ErrorType == ConversionErrorType.NonNullablePropertyNotSet), Is.True);

                return queryResult;
            },
            new JsonFilePath("ConversionErrorTypeConfigurations.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullablePropertyNotSet error type reported as warning
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonToObjectConversion.JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            // We can provide multiple instances of ConversionErrorTypeConfiguration
                            // to configure different error types
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                Assert.That(queryResult.Value, Is.Not.Null);

                Assert.That(queryResult.ErrorsAndWarnings.ConversionErrors.Errors.All(x => x.ErrorType != ConversionErrorType.NonNullablePropertyNotSet), Is.True);
                Assert.That(queryResult.ErrorsAndWarnings.ConversionWarnings.Errors.Any(
                    x => x.ErrorType == ConversionErrorType.NonNullablePropertyNotSet), Is.True);

                return queryResult;
            },
            new JsonFilePath("ConversionErrorTypeConfigurations_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullablePropertyNotSet error type ignored
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonToObjectConversion.JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            // We can provide multiple instances of ConversionErrorTypeConfiguration
                            // to configure different error types
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.Ignore)
                        }
                    });

                Assert.That(queryResult.Value, Is.Not.Null);

                Assert.That(!queryResult.ErrorsAndWarnings.ConversionErrors.Errors.Any(), Is.True);
                Assert.That(queryResult.ErrorsAndWarnings.ConversionWarnings.Errors.All(
                    x => x.ErrorType != ConversionErrorType.NonNullablePropertyNotSet), Is.True);

                //ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.NonNullablePropertyNotSet);
                return queryResult;
            },
            new JsonFilePath("ConversionErrorTypeConfigurations_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task FailOnFirstError_Test()
    {
        var employeesInAllCompaniesQueryExceptOneQuery = "Companies.Select(c => c.Employees.Where(x => x.Id != 100000001))";
        
        // A query which will result in errors. If we do not override FailOnFirstError configuration
        // the result will be null
        await ValidateQueryResultAsync(employeesInAllCompaniesQueryExceptOneQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData);

                Assert.That(queryResult.Value, Is.Null);
                Assert.That(queryResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(1));

                return queryResult;
            },
            new JsonFilePath("FailOnFirstError_True.json", TestExpectedResultFilesRelativePath));

        // Same query with true value used for FailOnFirstError configuration
        // The result will have errors, but the conversion will not stop on first error.
        await ValidateQueryResultAsync(employeesInAllCompaniesQueryExceptOneQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonToObjectConversion.JsonConversionSettingsOverrides
                    {
                        FailOnFirstError = false
                    });

                Assert.That(queryResult.Value, Is.Not.Null);
                Assert.That(queryResult.Value.Count, Is.EqualTo(11));
                Assert.That(queryResult.ErrorsAndWarnings.ConversionErrors.Errors.Count, Is.EqualTo(3));

                return queryResult;
            },
            new JsonFilePath("FailOnFirstError_False.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task TryMapJsonConversionType_Test()
    {
        var employeesQuery = "Employees";
       
        await ValidateQueryResultAsync(employeesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) =>
            {
                // public delegate Type? TryMapTypeDelegate(Type defaultTypeToConvertParsedJsonTo, IParsedJson convertedParsedJson);
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonToObjectConversion.JsonConversionSettingsOverrides
                    {
                        TryMapJsonConversionType = (defaultTypeToConvertParsedJsonTo, convertedParsedJson) =>
                        {
                            if (convertedParsedJson.HasKey(nameof(EmployeeWithSsn.Ssn)))
                                return typeof(EmployeeWithSsn);

                            return defaultTypeToConvertParsedJsonTo;
                        },
                        FailOnFirstError = false
                    });

                Assert.That(queryResult.Value, Is.Not.Null);
                Assert.That(queryResult.Value.Count, Is.EqualTo(2));

                Assert.That(queryResult.Value[0].GetType() == typeof(Employee));

                var employeeWithSsn = queryResult.Value[1] as EmployeeWithSsn;


                Assert.That(employeeWithSsn, Is.Not.Null);
                Assert.That(employeeWithSsn.Ssn, Is.EqualTo("111-222-33333"));
                return queryResult;
            },
            new JsonFilePath("TryMapJsonConversionType.json", TestExpectedResultFilesRelativePath));

    }

    [Test]
    public async Task JsonPropertyFormat_Properties_Initialized_From_CamelCase_Json_Test()
    {
        var employeesQuery = "Employees";

        // JSON in JsonFile3.json uses camel case format field names.
        await ValidateQueryResultAsync(employeesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile3.json"),

            (query, jsonTextData) =>
            {
                // public delegate Type? TryMapTypeDelegate(Type defaultTypeToConvertParsedJsonTo, IParsedJson convertedParsedJson);
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonToObjectConversion.JsonConversionSettingsOverrides
                    {
                        JsonPropertyFormat = JsonPropertyFormat.CamelCase
                    });

                Assert.That(queryResult.Value, Is.Not.Null);
                Assert.That(queryResult.Value.Count, Is.EqualTo(2));
                Assert.That(queryResult.HasErrors(), Is.False);
                return queryResult;
            },
            new JsonFilePath("JsonPropertyFormat_Properties_Initialized_From_CamelCase_Json.json", TestExpectedResultFilesRelativePath));
    }
}