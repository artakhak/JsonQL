namespace JsonQL.Tests.ContextValues;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task CollectionItemIndexExpressionsTest()
    {
        await DoSuccessfulTest(["ContextValues", "CollectionItemIndexExpressions"], 
            "JsonFile2.json", "JsonFile1.json");
    }
}