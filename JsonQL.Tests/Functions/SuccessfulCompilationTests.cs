namespace JsonQL.Tests.Functions;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task AssertHasValueTest()
    {
        await DoSuccessfulTest(["Functions", "AssertHasValue"], "JsonFile1.json");
    }
}