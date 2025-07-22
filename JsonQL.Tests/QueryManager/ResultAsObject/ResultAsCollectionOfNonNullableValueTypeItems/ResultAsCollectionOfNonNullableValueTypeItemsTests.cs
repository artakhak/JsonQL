using JsonQL.Query;

namespace JsonQL.Tests.QueryManager.ResultAsObject.ResultAsCollectionOfNonNullableValueTypeItems;

[TestFixture]
public class ResultAsCollectionOfNonNullableValueTypeItemsTests : ResultValidatingTestsAbstr
{
    
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNonNullableValueTypeItems", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNonNullableValueTypeItems", "ExpectedResults"];

    private const string SelectSalariesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

    [Test]
    public Task QueryCollection_For_NonNullable_Array_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
             // The call to QueryManager.QueryObject<double[]> below returns double[] in IObjectQueryResult<double[]>.Value
             QueryManager.QueryObject<double[]>(query, jsonTextData, [false, false]),
            new JsonFilePath("QueryCollection_For_NonNullable_Array_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_NonNullable_List_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
                QueryManager.QueryObject<List<double>>(query, jsonTextData, [false, false]),
            new JsonFilePath("QueryCollection_For_NonNullable_List_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_NonNullable_IReadOnlyList_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
                QueryManager.QueryObject<IReadOnlyList<double>>(query, jsonTextData, [false, false]),
            new JsonFilePath("QueryCollection_For_NonNullable_IReadOnlyList_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_NonNullable_IEnumerable_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
                QueryManager.QueryObject<IEnumerable<double>>(query, jsonTextData, [false, false]),
            new JsonFilePath("QueryCollection_For_NonNullable_IEnumerable_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }
}
