namespace JsonQL.Tests.JsonMutators.Value;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task ReferenceJsonObjectsInParentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "Value", "ReferenceJsonObjectsInParentJson"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task ReferenceJsonObjectsInParentsParentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "Value", "ReferenceJsonObjectsInParentsParentJson"], "JsonFile3.json",
            "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task ReferenceJsonObjectsInCurrentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "Value", "ReferenceJsonObjectsInCurrentJson"], "JsonFile1.json");
    }

    [Test]
    public async Task InvalidPathsTest()
    {
        await DoSuccessfulTest(["JsonMutators", "Value", "InvalidPaths"], "JsonFile1.json");
    }

    [Test]
    public async Task CollectionsAndSingleValuesTest()
    {
        string[] testFolderPath = ["JsonMutators", "Value", "CollectionsAndSingleValues"];
        await DoSuccessfulTest(new ResourcePath("JsonFile2.json",
                testFolderPath),
            new ResourcePath("JsonFile1.json",
                testFolderPath),
            new ResourcePath("CompaniesAsArrayInJson.json", ["SharedJsonFiles"]));
    }
}