namespace JsonQL.Tests.FutureReleases.Grouping;

[Ignore("Remove ignore once the story https://artakhak.atlassian.net/browse/JE-3 is implemented")]
[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task GroupingTest()
    {
        await DoSuccessfulTest(new ResourcePath("JsonFile1.json", ["FutureReleases", "Grouping"]),
            new ResourcePath("Companies.json", ["FutureReleases", "Grouping"]));
    }
}
