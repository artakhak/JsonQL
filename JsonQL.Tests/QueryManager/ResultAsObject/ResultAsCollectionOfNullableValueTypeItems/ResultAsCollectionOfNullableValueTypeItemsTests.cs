using JsonQL.Query;

namespace JsonQL.Tests.QueryManager.ResultAsObject.ResultAsCollectionOfNullableValueTypeItems;

[TestFixture]
public class ResultAsCollectionOfNullableValueTypeItemsTests : ResultValidatingTestsAbstr
{
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNullableValueTypeItems", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNullableValueTypeItems", "ExpectedResults"];

    private const string SelectAgesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Age))";

    [Test]
    public Task QueryCollection_For_Nullable_Array_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAgesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),
            (query, jsonTextData) =>
             // The call to QueryManager.QueryObject<double?[]> below returns double?[] in IObjectQueryResult<double?[]>.Value
             QueryManager.QueryObject<double?[]>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_Array_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_Nullable_List_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAgesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),
            (query, jsonTextData) =>
                QueryManager.QueryObject<List<double?>>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_List_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_Nullable_IReadOnlyList_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAgesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),
            (query, jsonTextData) =>
                QueryManager.QueryObject<IReadOnlyList<double?>>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_IReadOnlyList_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_Nullable_IEnumerable_Of_ValueType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAgesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),
            (query, jsonTextData) =>
                QueryManager.QueryObject<IEnumerable<double?>>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_IEnumerable_Of_ValueType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }
}