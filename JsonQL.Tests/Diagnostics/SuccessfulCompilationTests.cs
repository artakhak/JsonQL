namespace JsonQL.Tests.Diagnostics;

//[Ignore("Diagnostics")]
[TestFixture]
public class DiagnosticsTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task DiagnosticsTest()
    {
        await DoSuccessfulTest(["Diagnostics"], "JsonFile2.json", "JsonFile1.json");
    }
}