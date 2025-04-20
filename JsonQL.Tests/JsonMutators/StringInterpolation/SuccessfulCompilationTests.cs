namespace JsonQL.Tests.JsonMutators.StringInterpolation;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task ReferenceValueFromParentTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "ReferenceValueFromParent"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task ReferenceValueFromCurrentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "ReferenceValueFromCurrentJson"], "JsonFile1.json");
    }

    [Test]
    public async Task ReferenceValueUsingMultipleIndexesTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "ReferenceValueUsingMultipleIndexes"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task DefaultValueCasesReferencingCurrentJsonJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "DefaultValueCasesReferencingCurrentJson"], "JsonFile1.json");
    }

    [Test]
    public async Task DefaultValueCasesReferencingParentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "DefaultValueCasesReferencingParentJson"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task ComplexDefaultValueCasesTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "ComplexDefaultValueCases"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task DefaultValuesInExpressionsTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "DefaultValuesInExpressions"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task QueryExpressionsInJsonValuesTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "QueryExpressionsInJsonValues"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task BracesWithOperatorsTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "BracesWithOperators"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonSimpleValueMutatorStringConversionTest()
    {
        await DoSuccessfulTest(["JsonMutators", "StringInterpolation", "JsonSimpleValueMutatorStringConversion"], "JsonFile1.json");
    }
}