using JsonQL.JsonToObjectConversion;
using JsonQL.Query;
using JsonQL.Tests.QueryManager.ResultAsObject.ConversionErrors.Models;

namespace JsonQL.Tests.QueryManager.ResultAsObject.ConversionErrors;

[TestFixture]
public class ConversionErrorsTests : ResultValidatingTestsAbstr
{
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "ConversionErrors", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "ConversionErrors", "ExpectedResults"];


    [Test]
    public async Task Non_Nullable_Reference_Type_Property_Not_Set_Test()
    {
        var employeesWithNullAddressQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Address is null)))";

        // A query which fails with NonNullablePropertyNotSet errors
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData);
                ValidateConversionErrorIsReportedAtErrorLevel(queryResult, ConversionErrorType.NonNullablePropertyNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Reference_Type_Property_Not_Set.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullablePropertyNotSet error type reported as warning
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                ValidateConversionErrorIsReportedAtWarnLevel(queryResult, ConversionErrorType.NonNullablePropertyNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Reference_Type_Property_Not_Set_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullablePropertyNotSet error type ignored
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.Ignore)
                        }
                    });

                ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.NonNullablePropertyNotSet);
                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Reference_Type_Property_Not_Set_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task Non_Nullable_Value_Type_Property_Not_Set_Test()
    {
        var employeesWithNullAddressQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Age is null)))";

        // A query which fails with NonNullablePropertyNotSet errors
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData);
                ValidateConversionErrorIsReportedAtErrorLevel(queryResult, ConversionErrorType.NonNullablePropertyNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Value_Type_Property_Not_Set.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullablePropertyNotSet error type reported as warning
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                ValidateConversionErrorIsReportedAtWarnLevel(queryResult, ConversionErrorType.NonNullablePropertyNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Value_Type_Property_Not_Set_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullablePropertyNotSet error type ignored
        await ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullablePropertyNotSet, ErrorReportingType.Ignore)
                        }
                    });

                ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.NonNullablePropertyNotSet);
                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Value_Type_Property_Not_Set_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task Non_Nullable_Collection_Item_Value_Not_Set_In_Return_Value_Test()
    {
        var collectionsOfCollectionsQuery = "ArraysOfArrays1";

        // A query which fails with NonNullableCollectionItemValueNotSet errors
        await ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IReadOnlyList<IEmployee[]>>>(query, jsonTextData);
                ValidateConversionErrorIsReportedAtErrorLevel(queryResult, ConversionErrorType.NonNullableCollectionItemValueNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Collection_Item_Value_Not_Set_In_Return_Value.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullableCollectionItemValueNotSet error type reported as warning
        await ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IReadOnlyList<IEmployee[]>>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                ValidateConversionErrorIsReportedAtWarnLevel(queryResult, ConversionErrorType.NonNullableCollectionItemValueNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Collection_Item_Value_Not_Set_In_Return_Value_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullableCollectionItemValueNotSet error type ignored
        await ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<List<IReadOnlyList<IEmployee[]>>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, ErrorReportingType.Ignore)
                        }
                    });

                ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.NonNullableCollectionItemValueNotSet);
                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Collection_Item_Value_Not_Set_In_Return_Value_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task Non_Nullable_Collection_Item_Value_Not_Set_In_Property_Test()
    {
        var collectionsOfCollectionsQuery = "TestObjects";

        // A query which fails with NonNullableCollectionItemValueNotSet errors
        await ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "ObjectsWithCollectionsOfCollectionsProperties.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IReadOnlyList<TestClassWithCollectionOfCollectionsProperty>>(query, jsonTextData);
                ValidateConversionErrorIsReportedAtErrorLevel(queryResult, ConversionErrorType.NonNullableCollectionItemValueNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Collection_Item_Value_Not_Set_In_Property.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullableCollectionItemValueNotSet error type reported as warning
        await ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "ObjectsWithCollectionsOfCollectionsProperties.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IReadOnlyList<TestClassWithCollectionOfCollectionsProperty>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                ValidateConversionErrorIsReportedAtWarnLevel(queryResult, ConversionErrorType.NonNullableCollectionItemValueNotSet);

                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Collection_Item_Value_Not_Set_In_Property_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with NonNullableCollectionItemValueNotSet error type ignored
        await ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "ObjectsWithCollectionsOfCollectionsProperties.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IReadOnlyList<TestClassWithCollectionOfCollectionsProperty>>(query, jsonTextData, null,
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.NonNullableCollectionItemValueNotSet, ErrorReportingType.Ignore)
                        }
                    });

                ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.NonNullableCollectionItemValueNotSet);
                return queryResult;
            },
            new JsonFilePath("Non_Nullable_Collection_Item_Value_Not_Set_In_Property_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task Value_Type_Value_Not_Set_Test()
    {
        // A query that returns null
        var firstNullAgeQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Age is null).Select(x => null))).First()";
        
        // A query which fails with ValueNotSet errors
        await ValidateQueryResultAsync(firstNullAgeQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<double>(query, jsonTextData, [false]);
                ValidateConversionErrorIsReportedAtErrorLevel(queryResult, ConversionErrorType.ValueNotSet);

                return queryResult;
            },
            new JsonFilePath("Value_Type_Value_Not_Set.json", TestExpectedResultFilesRelativePath));

        // Same query with ValueNotSet error type reported as warning
        await ValidateQueryResultAsync(firstNullAgeQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<double>(query, jsonTextData, [false],
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                ValidateConversionErrorIsReportedAtWarnLevel(queryResult, ConversionErrorType.ValueNotSet);

                return queryResult;
            },
            new JsonFilePath("Value_Type_Value_Not_Set_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with ValueNotSet error type ignored
        await ValidateQueryResultAsync(firstNullAgeQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<double>(query, jsonTextData, [false],
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, ErrorReportingType.Ignore)
                        }
                    });

                ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.ValueNotSet);
                return queryResult;
            },
            new JsonFilePath("Value_Type_Value_Not_Set_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task Reference_Type_Value_Not_Set_Test()
    {
        // A query that returns null
        var employeesNotFoundQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Age == 1000))).First()";

        // A query which fails with ValueNotSet errors
        await ValidateQueryResultAsync(employeesNotFoundQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IEmployee>(query, jsonTextData, [false]);
                ValidateConversionErrorIsReportedAtErrorLevel(queryResult, ConversionErrorType.ValueNotSet);

                return queryResult;
            },
            new JsonFilePath("Reference_Type_Value_Not_Set.json", TestExpectedResultFilesRelativePath));

        // Same query with ValueNotSet error type reported as warning
        await ValidateQueryResultAsync(employeesNotFoundQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IEmployee>(query, jsonTextData, [false],
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                ValidateConversionErrorIsReportedAtWarnLevel(queryResult, ConversionErrorType.ValueNotSet, false);

                return queryResult;
            },
            new JsonFilePath("Reference_Type_Value_Not_Set_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with ValueNotSet error type ignored
        await ValidateQueryResultAsync(employeesNotFoundQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IEmployee>(query, jsonTextData, [false],
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, ErrorReportingType.Ignore)
                        }
                    });

                ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.ValueNotSet, false);
                return queryResult;
            },
            new JsonFilePath("Reference_Type_Value_Not_Set_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task Cannot_Create_Instance_of_Class_Test()
    {
        // A query that returns null
        var employeesNotFoundQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Age is not null))).First()";

        // A query which fails with CannotCreateInstanceOfClass errors
        await ValidateQueryResultAsync(employeesNotFoundQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<EmployeeAbstr>(query, jsonTextData, [false]);
                ValidateConversionErrorIsReportedAtErrorLevel(queryResult, ConversionErrorType.CannotCreateInstanceOfClass);

                return queryResult;
            },
            new JsonFilePath("Cannot_Create_Instance_of_Class.json", TestExpectedResultFilesRelativePath));

        // Same query with CannotCreateInstanceOfClass error type reported as warning
        await ValidateQueryResultAsync(employeesNotFoundQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<EmployeeAbstr>(query, jsonTextData, [false],
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.CannotCreateInstanceOfClass, ErrorReportingType.ReportAsWarning),
                            new ConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, ErrorReportingType.ReportAsWarning)
                        }
                    });

                ValidateConversionErrorIsReportedAtWarnLevel(queryResult, ConversionErrorType.CannotCreateInstanceOfClass, false);

                return queryResult;
            },
            new JsonFilePath("Cannot_Create_Instance_of_Class_Error_Type_Reported_as_Warning.json", TestExpectedResultFilesRelativePath));

        // Same query with CannotCreateInstanceOfClass error type ignored
        await ValidateQueryResultAsync(employeesNotFoundQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<EmployeeAbstr>(query, jsonTextData, [false],
                    new JsonConversionSettingsOverrides
                    {
                        ConversionErrorTypeConfigurations = new List<IConversionErrorTypeConfiguration>
                        {
                            new ConversionErrorTypeConfiguration(ConversionErrorType.CannotCreateInstanceOfClass, ErrorReportingType.Ignore),
                            new ConversionErrorTypeConfiguration(ConversionErrorType.ValueNotSet, ErrorReportingType.Ignore)
                        }
                    });

                ValidateConversionErrorIsIgnored(queryResult, ConversionErrorType.CannotCreateInstanceOfClass, false);
                return queryResult;
            },
            new JsonFilePath("Cannot_Create_Instance_of_Class_Error_Type_Turned_Off.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Failed_to_Convert_Json_Value_to_ExpectedType_Test()
    {
        throw new NotImplementedException();
    }

    [Test]
    public Task Generic_Conversion_Error_Test()
    {
        throw new NotImplementedException();
    }

    private void ValidateConversionErrorIsReportedAtErrorLevel<T>(IObjectQueryResult<T> queryResult, ConversionErrorType conversionErrorType)
    {
        if (queryResult.Value != null)
            Assert.That(queryResult.Value.GetType().IsValueType, Is.True);

        Assert.That(queryResult.ErrorsAndWarnings.ConversionErrors.Errors.Any(
            x => x.ErrorType == conversionErrorType), Is.True);
        Assert.That(queryResult.ErrorsAndWarnings.ConversionWarnings.Errors.All(
            x => x.ErrorType == conversionErrorType), Is.True);
    }

    private void ValidateConversionErrorIsReportedAtWarnLevel<T>(IObjectQueryResult<T> queryResult, ConversionErrorType conversionErrorType,
        bool valueIsNotNull = true)
    {
        Assert.That(queryResult.Value, valueIsNotNull ? Is.Not.Null : Is.Null);

        Assert.That(queryResult.ErrorsAndWarnings.ConversionErrors.Errors.All(x => x.ErrorType != conversionErrorType), Is.True);
        Assert.That(queryResult.ErrorsAndWarnings.ConversionWarnings.Errors.Any(
            x => x.ErrorType == conversionErrorType), Is.True);
    }
    private void ValidateConversionErrorIsIgnored<T>(IObjectQueryResult<T> queryResult, ConversionErrorType conversionErrorType,
        bool valueIsNotNull = true)
    {
        Assert.That(queryResult.Value, valueIsNotNull ? Is.Not.Null : Is.Null);

        Assert.That(!queryResult.ErrorsAndWarnings.ConversionErrors.Errors.Any(), Is.True);
        Assert.That(queryResult.ErrorsAndWarnings.ConversionWarnings.Errors.All(
            x => x.ErrorType != conversionErrorType), Is.True);
    }
}