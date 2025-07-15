using JsonQL.Query;

namespace JsonQL.Tests.QueryManager.ResultAsObject;

[TestFixture]
public class ResultAsObjectTests: ResultValidatingTestsAbstr
{
    // Tests to include
    // Example of value type item collections of type T[], IReadOnlyList<T>, IEnumerable<T>, List<T>
    // Example of nullable value type item collections of type T[], IReadOnlyList<T>, IEnumerable<T>, List<T>

    // Example of reference type item collections of type T[], IReadOnlyList<T>, IENumerable<T>, List<T>
    // Example of nullable reference type item collections of type T[], IReadOnlyList<T>, IENumerable<T>, List<T>

    // Example of single value type returned
    // Example of single nullable value type returned

    // Example of single ref type returned
    // Example of single nullable ref type returned

    // Example of non-nullable property values 
    // Example of nullable property value (Address: Address?, Age: int?)

    // Example of collection of collections with second higher level items being nullable
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "ExpectedResults"];

    [Test]
    public Task QueryCollectionOf_NonNullable_ValueTypeValues_Array_Test()
    {
        var selectSalariesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Salary))";
        return this.ValidateQueryResultAsync(selectSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) =>
             // The call to QueryManager.QueryObject<double[]> below returns double[]
             QueryManager.QueryObject<double[]>(query, jsonTextData),
            new JsonFilePath("QueryCollectionOf_NonNullable_ValueTypeValues_Array_Test_ExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollectionOf_Nullable_ValueTypeValues_Array_Test()
    {
        var nullableDoublesQuery = "NumericValues.Where(x => x != 15)";
        return this.ValidateQueryResultAsync(nullableDoublesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) =>
                // The call to QueryManager.QueryObject<double[]> below returns double[]
            {
                var result = QueryManager.QueryObject<double?[]>(query, jsonTextData, [false, true]);

                // Demo
                double?[] parsedCollection = result.Value!;
                return result;
            },
            new JsonFilePath("QueryCollectionOf_Nullable_ValueTypeValues_Array_Test_ExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollectionOf_Nullable_Strings_Array_Test()
    {
        var nullableStringsQuery = "Texts.Where(x => x != 'Test 3')";
        return this.ValidateQueryResultAsync(nullableStringsQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) =>
            {
                var result = QueryManager.QueryObject<string?[]>(query, jsonTextData, [false, true]);
                Assert.That(result.Value?.Any(x => x is null), Is.True);
                return result;
            },
            new JsonFilePath("QueryCollectionOf_Nullable_ValueTypeValues_Array_Test_ExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollectionOfValueTypeValues_IReadOnlyList_Test()
    {
        var selectSalariesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Salary))";
        return this.ValidateQueryResultAsync(selectSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),

            (query, jsonTextData) =>
                // The call to QueryManager.QueryObject<double[]> below returns double[]
                QueryManager.QueryObject<IReadOnlyList<double>>(query, jsonTextData),
            new JsonFilePath("QueryCollectionOfValueTypeValues_IReadOnlyList_Test_ExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    /*[Test]
    public async Task QueryListOfValueTypeValuesTest()
    {
        var selectSalariesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        await this.ValidateQueryResultAsync(selectSalariesOfAllEmployeesInAllCompaniesQuery, 
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            // We can replace the next line with QueryManager.QueryJsonValue, however showing
            // the parameters being passed might serve as a better demo
            (query, jsonTextData) => this.QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QueryListOfValueTypeValuesTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task QuerySingleValueTypeTest()
    {
        var selectSumOfAllSalariesOfAllEmployeesInAllCompaniesQuery = "Sum(Companies.Select(x => x.Employees.Select(x => x.Salary)))";

        await this.ValidateQueryResultAsync(selectSumOfAllSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            // We can replace the next line with QueryManager.QueryJsonValue, however showing
            // the parameters being passed might serve as a better demo
            (query, jsonTextData) => this.QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QuerySingleValueTypeTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task QueryListOfReferenceTypeValuesTest()
    {
        var selectAddressesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Address))";
        await this.ValidateQueryResultAsync(selectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) => this.QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QueryListOfReferenceTypeValuesTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task QuerySingleReferenceTypeValueTest()
    {
        var selectAddressesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Address)).First()";
        await this.ValidateQueryResultAsync(selectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) => this.QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QuerySingleReferenceTypeTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public async Task QueryObjectsInParentJsonFileTest()
    {
        var selectSalariesOfAllEmployeesInAllCompaniesInParentFileQuery = "CompaniesInParentJson.Select(x => x.Employees.Select(x => x.Salary))";

        await this.ValidateQueryResultAsync(selectSalariesOfAllEmployeesInAllCompaniesInParentFileQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json", "JsonFile1.json"),
            (query, jsonTextData) => this.QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QueryObjectsInParentJsonFileTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }*/
}