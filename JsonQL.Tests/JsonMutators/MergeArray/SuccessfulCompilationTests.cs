namespace JsonQL.Tests.JsonMutators.MergeArray;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task MergeCollectionIntoArrayFromParentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "MergeArray", "MergeCollectionIntoArrayFromParentJson"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task MergeCollectionIntoArrayFromCurrentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "MergeArray", "MergeCollectionIntoArrayFromCurrentJson"], "JsonFile1.json");
    }

    [Test]
    public async Task MultipleArrayIndexesTest()
    {
        await DoSuccessfulTest(["JsonMutators", "MergeArray", "MultipleArrayIndexes"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task MergeCollectionIntoArrayByWhereClause1Test()
    {
        await DoSuccessfulTest(["JsonMutators", "MergeArray", "MergeCollectionIntoArrayByWhereClause1"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task MergeCollectionIntoArrayByWhereClause2Test()
    {
        await DoSuccessfulTest(["JsonMutators", "MergeArray", "MergeCollectionIntoArrayByWhereClause2"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task InvalidPathsTest()
    {
        await DoSuccessfulTest(["JsonMutators", "MergeArray", "InvalidPaths"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task MergeCollectionsAndSingleValuesTest()
    {
        string[] testFolderPath = ["JsonMutators", "MergeArray", "MergeCollectionsAndSingleValues"];
        await DoSuccessfulTest(new ResourcePath("JsonFile2.json",
                testFolderPath),
            new ResourcePath("JsonFile1.json",
                testFolderPath),
            new ResourcePath("CompaniesAsArrayInJson.json", ["SharedJsonFiles"]));
    }
}
