using JsonQL.Query;
using JsonQL.Tests.QueryManager.ResultAsObject.Models;

namespace JsonQL.Tests.QueryManager.ResultAsObject.ResultAsCollectionOfNullableReferenceItems;

[TestFixture]
public class ResultAsCollectionOfNullableReferenceItemsTests : ResultValidatingTestsAbstr
{
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNullableReferenceItems", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNullableReferenceItems", "ExpectedResults"];

    private const string SelectAddressesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Address))";
    [Test]
    public Task QueryCollection_For_Nullable_Array_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
             // The call to QueryManager.QueryObject<IAddress?[]> below returns IAddress?[] in IObjectQueryResult<IAddress?[]>.Value
             QueryManager.QueryObject<IAddress?[]>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_Array_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_Nullable_List_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),
            (query, jsonTextData) =>
                QueryManager.QueryObject<List<IAddress?>>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_List_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_Nullable_IReadOnlyList_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),
            (query, jsonTextData) =>
                QueryManager.QueryObject<IReadOnlyList<IAddress?>>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_IReadOnlyList_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_NonNullable_IEnumerable_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),
            (query, jsonTextData) =>
                QueryManager.QueryObject<IEnumerable<IAddress?>>(query, jsonTextData, [false, true]),
            new JsonFilePath("QueryCollection_For_Nullable_IEnumerable_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }
}