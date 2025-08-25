namespace JsonQL.Tests.FutureReleases.JsonValuePathElements.Sort;

[Ignore("Remove ignore once the story https://artakhak.atlassian.net/browse/JE-1 is implemented")]
[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task JsonValuePathElements_Sort()
    {
        await DoSuccessfulTest(new ResourcePath("JsonFile1.json", ["FutureReleases", "JsonValuePathElements", "Sort"]),
            new ResourcePath("CompaniesAsArrayInJson.json", ["SharedJsonFiles"]));
    }
}
