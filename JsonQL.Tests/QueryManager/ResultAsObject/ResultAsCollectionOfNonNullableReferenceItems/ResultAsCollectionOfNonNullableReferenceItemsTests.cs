using JsonQL.Query;
using JsonQL.Tests.QueryManager.ResultAsObject.Models;

namespace JsonQL.Tests.QueryManager.ResultAsObject.ResultAsCollectionOfNonNullableReferenceItems;

[TestFixture]
public class ResultAsObjectTests : ResultValidatingTestsAbstr
{
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNonNullableReferenceItems", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsObject", "ResultAsCollectionOfNonNullableReferenceItems", "ExpectedResults"];

    private const string SelectNonNullAddressesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x)).Where(x => x.Address is not null)";

    [Test]
    public Task QueryCollection_For_NonNullable_Array_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectNonNullAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
             // The call to QueryManager.QueryObject<IAddress?[]> below returns IEmployee[] in IObjectQueryResult<IEmployee[]>.Value
             QueryManager.QueryObject<IEmployee[]>(query, jsonTextData),
            new JsonFilePath("QueryCollection_For_NonNullable_Array_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_NonNullable_List_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectNonNullAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
                QueryManager.QueryObject<List<IEmployee>>(query, jsonTextData),
            new JsonFilePath("QueryCollection_For_NonNullable_List_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_NonNullable_IReadOnlyList_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectNonNullAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
                QueryManager.QueryObject<IReadOnlyList<IEmployee>>(query, jsonTextData),
            new JsonFilePath("QueryCollection_For_NonNullable_IReadOnlyList_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryCollection_For_NonNullable_IEnumerable_of_ReferenceType_Items_Test()
    {
        return this.ValidateQueryResultAsync(SelectNonNullAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile1.json"),

            (query, jsonTextData) =>
                QueryManager.QueryObject<IEnumerable<IEmployee>>(query, jsonTextData),
            new JsonFilePath("QueryCollection_For_NonNullable_IEnumerable_of_ReferenceType_Items_Test.json", TestExpectedResultFilesRelativePath));
    }
}