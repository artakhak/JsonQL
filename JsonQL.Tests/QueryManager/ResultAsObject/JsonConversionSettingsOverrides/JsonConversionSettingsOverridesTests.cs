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
        var allEmployeesInAllCompaniesQuery = "Companies.Select(c => c.Employees.Where(x => x.Id != 100000001))";
        // var allEmployeesInAllCompaniesQuery = "Employees";

        // A query which will result in errors. If we do not override FailOnFirstError configuration
        // the result will be null
        await ValidateQueryResultAsync(allEmployeesInAllCompaniesQuery,
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
        await ValidateQueryResultAsync(allEmployeesInAllCompaniesQuery,
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

    //[Test]
    //public async Task Query_Json_InParent_Json_File_Test()
    //{
    //    var parentJsonFile = new JsonTextData(Guid.NewGuid().ToString(),
    //         ResourceFileLoader.LoadJsonFile(new ResourcePath("JsonFile1.json", TestDataFilesRelativePath)));

    //    var mainJsonFile = new JsonTextData(Guid.NewGuid().ToString(),
    //        ResourceFileLoader.LoadJsonFile(new ResourcePath("JsonFile2.json", TestDataFilesRelativePath)), parentJsonFile);

    //    // NOTE: CompaniesInParentJson is in JsonFile1.txt which is used as a parent json in JsonFile1 loaded into parentJsonFile
    //    var selectEmployeesInFilteredCompaniesOlderThan40Query =
    //        "CompaniesInParentJson.Where(x => x.CompanyData.Name != 'Tech Innovations, LLC').Select(c => c.Employees.Where(e => e.Age > 40))";

    //    var queryResult = QueryManager.QueryObject<IReadOnlyList<IEmployee>>(selectEmployeesInFilteredCompaniesOlderThan40Query, mainJsonFile);

    //    await JsonQLResultValidator.ValidateResultAsync(new JsonQLResultValidationParameters
    //    {
    //        GetJsonQlResultAsync = () => Task.FromResult<object>(queryResult),
    //        LoadExpectedResultJsonFileAsync = () => Task.FromResult(
    //            ResourceFileLoader.LoadJsonFile(new ResourcePath("Query_Json_InParent_Json_File_Test.json", TestExpectedResultFilesRelativePath)))
    //    });

    //    Assert.That(queryResult.Value, Is.InstanceOf<IReadOnlyList<IEmployee>>());
    //}

    //[Test]
    //public async Task Query_Json_InParent_Json_File_Test()
    //{
    //    var parentJsonFile = new JsonTextData(Guid.NewGuid().ToString(),
    //         ResourceFileLoader.LoadJsonFile(new ResourcePath("JsonFile1.json", TestDataFilesRelativePath)));

    //    var mainJsonFile = new JsonTextData(Guid.NewGuid().ToString(),
    //        ResourceFileLoader.LoadJsonFile(new ResourcePath("JsonFile2.json", TestDataFilesRelativePath)), parentJsonFile);

    //    // NOTE: CompaniesInParentJson is in JsonFile1.txt which is used as a parent json in JsonFile1 loaded into parentJsonFile
    //    var selectEmployeesInFilteredCompaniesOlderThan40Query =
    //        "CompaniesInParentJson.Where(x => x.CompanyData.Name != 'Tech Innovations, LLC').Select(c => c.Employees.Where(e => e.Age > 40))";

    //    var queryResult = QueryManager.QueryObject<IReadOnlyList<IEmployee>>(selectEmployeesInFilteredCompaniesOlderThan40Query, mainJsonFile);

    //    await JsonQLResultValidator.ValidateResultAsync(new JsonQLResultValidationParameters
    //    {
    //        GetJsonQlResultAsync = () => Task.FromResult<object>(queryResult),
    //        LoadExpectedResultJsonFileAsync = () => Task.FromResult(
    //            ResourceFileLoader.LoadJsonFile(new ResourcePath("Query_Json_InParent_Json_File_Test.json", TestExpectedResultFilesRelativePath)))
    //    });

    //    Assert.That(queryResult.Value, Is.InstanceOf<IReadOnlyList<IEmployee>>());
    //}

    //[Test]
    //public Task Query_for_ReferenceType_Implementation_Instance_Test()
    //{
    //    var selectFirstEmployeeInInFilteredCompaniesOlderThan40Query =
    //        "Companies.Where(x => x.CompanyData.Name != 'Strange Things, Inc').Select(c => c.Employees.Where(e => e.Age > 40)).First()";

    //    return ValidateQueryResultAsync(selectFirstEmployeeInInFilteredCompaniesOlderThan40Query,
    //        new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
    //        (query, jsonTextData) =>
    //        {
    //            var queryResult = QueryManager.QueryObject<Employee>(query, jsonTextData);

    //            // Note, this line is not necessary, but servers as a demo that 
    //            // queryResult.Value is of type Employee
    //            Employee? firstEmployee = queryResult.Value;
    //            Assert.That(firstEmployee, Is.Not.Null);
    //            return queryResult;
    //        },
    //        new JsonFilePath("Query_for_ReferenceType_Implementation_Instance_Test.json", TestExpectedResultFilesRelativePath));
    //}
}