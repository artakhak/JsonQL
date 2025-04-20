namespace JsonQL.Tests.OptionalAndNamedParameters;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task OptionalAndNamedParametersTest()
    {
        await DoSuccessfulTest(["OptionalAndNamedParameters"], "JsonFile1.json");
    }   
}