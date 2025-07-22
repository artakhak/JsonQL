namespace JsonQL.Tests.QueryManager.ResultAsParsedJsonValue.SuccessfulQueryResult;

[TestFixture]
public class ResultAsParsedJsonValueTests : ResultValidatingTestsAbstr
{
    private static readonly List<string> TestDataFilesRelativePath = ["QueryManager", "ResultAsParsedJsonValue", "SuccessfulQueryResult", "Data"];
    private static readonly List<string> TestExpectedResultFilesRelativePath = ["QueryManager", "ResultAsParsedJsonValue", "SuccessfulQueryResult", "ExpectedResults"];

    [Test]
    public Task QueryListOfValueTypeValuesTest()
    {
        var selectSalariesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Salary))";

        return ValidateQueryResultAsync(selectSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            // We can replace the next line with QueryManager.QueryJsonValue, however showing
            // the parameters being passed might serve as a better demo
            (query, jsonTextData) => QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QueryListOfValueTypeValuesTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QuerySingleValueTypeTest()
    {
        var selectSumOfAllSalariesOfAllEmployeesInAllCompaniesQuery = "Sum(Companies.Select(x => x.Employees.Select(x => x.Salary)))";

        return ValidateQueryResultAsync(selectSumOfAllSalariesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) => QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QuerySingleValueTypeTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryListOfReferenceTypeValuesTest()
    {
        var selectAddressesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Address))";
        return ValidateQueryResultAsync(selectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) => QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QueryListOfReferenceTypeValuesTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QuerySingleReferenceTypeValueTest()
    {
        var selectAddressesOfAllEmployeesInAllCompaniesQuery = "Companies.Select(x => x.Employees.Select(x => x.Address)).First()";
        return ValidateQueryResultAsync(selectAddressesOfAllEmployeesInAllCompaniesQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json"),
            (query, jsonTextData) => QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QuerySingleReferenceTypeTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }

    [Test]
    public Task QueryObjectsInParentJsonFileTest()
    {
        var selectSalariesOfAllEmployeesInAllCompaniesInParentFileQuery = "CompaniesInParentJson.Select(x => x.Employees.Select(x => x.Salary))";

        return ValidateQueryResultAsync(selectSalariesOfAllEmployeesInAllCompaniesInParentFileQuery,
            new TestJsonTextDataPath(TestDataFilesRelativePath, "JsonFile2.json", "JsonFile1.json"),
            (query, jsonTextData) => QueryManager.QueryJsonValue(query, jsonTextData),
            new JsonFilePath("QueryObjectsInParentJsonFileTestExpectedResult.json", TestExpectedResultFilesRelativePath));
    }
}
