namespace JsonQL.Tests.FutureReleases.MultilineQueries;

[Ignore("Remove ignore once the story https://artakhak.atlassian.net/browse/JE-7 is implemented")]
[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task MultilineQueriesTest()
    {
        await DoSuccessfulTest(new ResourcePath("JsonFile1.json", ["FutureReleases", "MultilineQueries"]),
            new ResourcePath("CompaniesAsArrayInJson.json", ["SharedJsonFiles"]));
    }
    
    /// <summary>
    /// Delete this test once the feature is implemented. Used for diagnostics.
    /// </summary>
    [Test]
    public async Task MultilineQueriesTest_Diagnostics()
    {
        await DoSuccessfulTest(new ResourcePath("JsonFile1_TempDiagnostics.json", ["FutureReleases", "MultilineQueries"]),
            new ResourcePath("CompaniesAsArrayInJson.json", ["SharedJsonFiles"]));
    }
}
