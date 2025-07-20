using JsonQL.Compilation;
using JsonQL.Diagnostics.ResultValidation;
using JsonQL.Query;
using JsonQL.Tests.QueryManager.ResultAsObject.Models;

namespace JsonQL.Tests.QueryManager.ResultAsObject.SuccessfulQueryResults;

[TestFixture]
public class JsonConversionSettingsOverridesTests : ResultValidatingTestsAbstr
{
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "SuccessfulQueryResults", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "SuccessfulQueryResults", "ExpectedResults"];

    [Test]
    public async Task Query_Json_InParent_Json_File_Test()
    {
        var parentJsonFile = new JsonTextData(Guid.NewGuid().ToString(),
             ResourceFileLoader.LoadJsonFile(new ResourcePath("JsonFile1.json", TestDataFilesRelativePath)));

        var mainJsonFile = new JsonTextData(Guid.NewGuid().ToString(),
            ResourceFileLoader.LoadJsonFile(new ResourcePath("JsonFile2.json", TestDataFilesRelativePath)), parentJsonFile);

        // NOTE: CompaniesInParentJson is in JsonFile1.txt which is used as a parent json in JsonFile1 loaded into parentJsonFile
        var selectEmployeesInFilteredCompaniesOlderThan40Query =
            "CompaniesInParentJson.Where(x => x.CompanyData.Name != 'Tech Innovations, LLC').Select(c => c.Employees.Where(e => e.Age > 40))";

        var queryResult = QueryManager.QueryObject<IReadOnlyList<IEmployee>>(selectEmployeesInFilteredCompaniesOlderThan40Query, mainJsonFile);

        await JsonQLResultValidator.ValidateResultAsync(new JsonQLResultValidationParameters
        {
            GetJsonQlResultAsync = () => Task.FromResult<object>(queryResult),
            LoadExpectedResultJsonFileAsync = () => Task.FromResult(
                ResourceFileLoader.LoadJsonFile(new ResourcePath("Query_Json_InParent_Json_File_Test.json", TestExpectedResultFilesRelativePath)))
        });

        Assert.That(queryResult.Value, Is.InstanceOf<IReadOnlyList<IEmployee>>());
    }

    [Test]
    public Task Query_for_ReferenceType_Implementation_Instance_Test()
    {
        var selectFirstEmployeeInInFilteredCompaniesOlderThan40Query =
            "Companies.Where(x => x.CompanyData.Name != 'Strange Things, Inc').Select(c => c.Employees.Where(e => e.Age > 40)).First()";

        return ValidateQueryResultAsync(selectFirstEmployeeInInFilteredCompaniesOlderThan40Query,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<Employee>(query, jsonTextData);

                // Note, this line is not necessary, but servers as a demo that 
                // queryResult.Value is of type Employee
                Employee? firstEmployee = queryResult.Value;
                Assert.That(firstEmployee, Is.Not.Null);
                return queryResult;
            },
            new JsonFilePath("Query_for_ReferenceType_Implementation_Instance_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Query_for_Interface_Instance_Test()
    {
        var selectFirstEmployeeInInFilteredCompaniesOlderThan40Query =
            "Companies.Where(x => x.CompanyData.Name != 'Strange Things, Inc').Select(c => c.Employees.Where(e => e.Age > 40)).First()";

        return ValidateQueryResultAsync(selectFirstEmployeeInInFilteredCompaniesOlderThan40Query,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IEmployee>(query, jsonTextData);

                // Note, this line is not necessary, but servers as a demo that 
                // queryResult.Value is of type IEmployee
                IEmployee? firstEmployee = queryResult.Value;
                Assert.That(firstEmployee, Is.Not.Null);
                return queryResult;
            },
            new JsonFilePath("Query_for_Interface_Instance_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Deserializing_Class_And_Interface_Property_Types_Test()
    {
        var selectFirstEmployeeInInFilteredCompaniesOlderThan40Query =
            "Companies.Select(c => c.Employees.Where(e => e.EmergencyContacts is not undefined)).First()";

        return ValidateQueryResultAsync(selectFirstEmployeeInInFilteredCompaniesOlderThan40Query,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) =>
            {
                var queryResult = QueryManager.QueryObject<IEmployee>(query, jsonTextData);

                Assert.That(queryResult.Value, Is.Not.Null);

                // Note, this line is not necessary, but servers as a demo that 
                // queryResult.Value is of type IEmployee,
                // firstEmployee.EmergencyContacts is of type IReadOnlyList<Address>
                // firstEmployee.Address is of type IAddress?
                IEmployee firstEmployee = queryResult.Value;
                IReadOnlyList<Address>? emergencyContacts = firstEmployee.EmergencyContacts;
                IAddress? address = firstEmployee.Address;
                return queryResult;
            },
            new JsonFilePath("Deserializing_Class_And_Interface_Property_Types_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Non_Nullable_Value_Type_Value_Test()
    {
        var selectAgeOfFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan100000_Query =
            "Companies.Where(x => x.CompanyData.Name != 'Strange Things, Inc').Select(c => c.Employees.Where(e => e.Salary >= 100000).Select(x => x.Age)).First()";

        return ValidateQueryResultAsync(selectAgeOfFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan100000_Query,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) => QueryManager.QueryObject<double>(query, jsonTextData),
            new JsonFilePath("Result_As_Non_Nullable_Value_Type_Value_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Nullable_Value_Type_Value_Test()
    {
        // The query returns null. No employee with salary greater than 1,000,000
        var selectAgeOfFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan1000000_Query =
            "Companies.Where(x => x.CompanyData.Name != 'Strange Things, Inc').Select(c => c.Employees.Where(e => e.Salary > 1000000).Select(x => x.Age)).First()";
        
        return ValidateQueryResultAsync(selectAgeOfFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan1000000_Query,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) => QueryManager.QueryObject<double?>(query, jsonTextData),
            new JsonFilePath("Result_As_Nullable_Value_Type_Value_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Non_Nullable_ReferenceType_Value_Test()
    {
        var selectFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan100000_Query =
            "Companies.Where(x => x.CompanyData.Name != 'Strange Things, Inc').Select(c => c.Employees.Where(e => e.Salary >= 100000)).First()";

        return ValidateQueryResultAsync(selectFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan100000_Query,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) => QueryManager.QueryObject<IEmployee>(query, jsonTextData),
            new JsonFilePath("Result_As_Non_Nullable_ReferenceType_Value_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Nullable_ReferenceType_Value_Test()
    {
        // The query returns null. No employee with salary greater than 1,000,000
        var selectFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan1000000_Query =
            "Companies.Where(x => x.CompanyData.Name != 'Strange Things, Inc').Select(c => c.Employees.Where(e => e.Salary >= 1000000)).First()";

        return ValidateQueryResultAsync(selectFirstEmployeeAcrossFilteredCompaniesWithSalaryGreaterThan1000000_Query,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) => QueryManager.QueryObject<IEmployee?>(query, jsonTextData),
            new JsonFilePath("Result_As_Nullable_ReferenceType_Value_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Reference_Type_Value_With_Null_Value_of_Property_of_Nullable_Value_Type_Test()
    {
        var employeesWithNullAgeQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Age is null)))";

        return ValidateQueryResultAsync(employeesWithNullAgeQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) => QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData),
            new JsonFilePath("Result_As_Reference_Type_Value_With_Null_Value_of_Property_of_Nullable_Value_Type_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Reference_Type_Value_With_Null_Value_of_Property_of_Nullable_Reference_Type_Test()
    {
        var employeesWithNullAddressQuery = "Companies.Select(c => c.Select(x => x.Employees.Where(x => x.Address is null)))";

        return ValidateQueryResultAsync(employeesWithNullAddressQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) => QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData),
            new JsonFilePath("Result_As_Reference_Type_Value_With_Null_Value_of_Property_of_Nullable_Reference_Type_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Non_Nullable_Collections_of_Collections_of_Collections_Test()
    {
        var collectionsOfCollectionsQuery = "ArraysOfArrays1.Where(x => x is not null && !Any(x, y => y is null || Any(y, z => z is null)))";

        return ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) => QueryManager.QueryObject<List<IReadOnlyList<IEmployee[]>>>(query, jsonTextData),
            new JsonFilePath("Result_As_Non_Nullable_Collections_of_Collections_of_Collections_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Nullable_Collections_of_Collections_of_Collections_Test()
    {
        // Will return null and will be converted to IReadOnlyList<List<IEmployee>>?
        // as a result of QueryManager.QueryObject<IReadOnlyList<List<IEmployee>>?>(query, jsonTextData)
        // in this test.
        var collectionsOfCollectionsQuery = "NonExistentArray";

        return ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) => 
                QueryManager.QueryObject<IReadOnlyList<List<IEmployee>>?>(query, jsonTextData, [true, false, false]),
            new JsonFilePath("Result_As_Nullable_Collections_of_Collections_of_Collections_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Collections_of_Collections_of_Collections_With_First_Level_Items_Nullable_Test()
    {
        var collectionsOfCollectionsQuery = "ArraysOfArrays1.Where(x => index == 0 || index == 1)";

        return ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) => QueryManager.QueryObject<List<IReadOnlyList<IEmployee[]>?>>(query, jsonTextData, 
                [false, true, false, false]),
            new JsonFilePath("Result_As_Collections_of_Collections_of_Collections_With_First_Level_Items_Nullable_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Collections_of_Collections_of_Collections_With_Second_Level_Items_Nullable_Test()
    {
        var collectionsOfCollectionsQuery = "ArraysOfArrays1.Where(x => index == 0 || index == 2)";

        return ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) => QueryManager.QueryObject<List<IReadOnlyList<IEmployee[]?>>>(query, jsonTextData,
                [false, false, true, false]),
            new JsonFilePath("Result_As_Collections_of_Collections_of_Collections_With_Second_Level_Items_Nullable_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task Result_As_Collections_of_Collections_of_Collections_With_Third_Level_Items_Nullable_Test()
    {
        var collectionsOfCollectionsQuery = "ArraysOfArrays1.Where(x => index == 0 || index == 3)";

        return ValidateQueryResultAsync(collectionsOfCollectionsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "CollectionsOfCollections.json"),

            (query, jsonTextData) => QueryManager.QueryObject<List<IReadOnlyList<IEmployee?[]>>>(query, jsonTextData,
                [false, false, false, true]),
            new JsonFilePath("Result_As_Collections_of_Collections_of_Collections_With_Third_Level_Items_Nullable_Test.json", TestExpectedResultFilesRelativePath));
    }
}