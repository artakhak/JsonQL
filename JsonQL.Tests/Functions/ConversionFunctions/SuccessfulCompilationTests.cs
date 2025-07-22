namespace JsonQL.Tests.Functions.ConversionFunctions;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task ConversionFunctions_ToBoolean_Test()
    {
        await DoSuccessfulTest(["Functions", "ConversionFunctions", "ToBoolean"], "JsonFile1.json");
    }

    [Test]
    public async Task ConversionFunctions_ToDouble_Test()
    {
        await DoSuccessfulTest(["Functions", "ConversionFunctions", "ToDouble"], "JsonFile1.json");
    }

    [Test]
    public async Task ConversionFunctions_ToInt_Test()
    {
        await DoSuccessfulTest(["Functions", "ConversionFunctions", "ToInt"], "JsonFile1.json");
    }

    [Test]
    public async Task ConversionFunctions_ToDateTime_Test()
    {
        await DoSuccessfulTest(["Functions", "ConversionFunctions", "ToDateTime"], "JsonFile1.json");
    }

    [Test]
    public async Task ConversionFunctions_ToDate_Test()
    {
        await DoSuccessfulTest(["Functions", "ConversionFunctions", "ToDate"], "JsonFile1.json");
    }

    [Test]
    public async Task ConversionFunctions_ToString_Test()
    {
        await DoSuccessfulTest(["Functions", "ConversionFunctions", "ToString"], "JsonFile1.json");
    }
}
