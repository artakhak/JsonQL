namespace JsonQL.Tests.Demo;

[TestFixture]
[Ignore("COMPLETE AND ENABLE")]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task AppSettingsDemoTest()
    {
        await DoSuccessfulTest(["Demo"], "AppSettings.json", "GlobalSettings.json");
    }
}