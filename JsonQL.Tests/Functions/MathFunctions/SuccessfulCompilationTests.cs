namespace JsonQL.Tests.Functions.MathFunctions;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task MathFunctions_IsEven_Tests()
    {
        await DoSuccessfulTest(["Functions", "MathFunctions", "IsEven"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task MathFunctions_IsOdd_Tests()
    {
        await DoSuccessfulTest(["Functions", "MathFunctions", "IsOdd"], "JsonFile2.json", "JsonFile1.json");
    }
}