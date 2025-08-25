namespace JsonQL.Tests.FutureReleases.ComplexProjections;

[Ignore("Remove ignore once the story https://artakhak.atlassian.net/browse/JE-2 is implemented")]
[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task ComplexProjectionsTest()
    {
        await DoSuccessfulTest(new ResourcePath("JsonFile1.json", ["FutureReleases", "ComplexProjections"]),
            new ResourcePath("Companies.json", ["FutureReleases", "ComplexProjections"]));
    }
}
