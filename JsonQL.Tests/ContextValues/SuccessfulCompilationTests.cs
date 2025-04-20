namespace JsonQL.Tests.ContextValues;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task CollectionItemIndexExpressionsTest()
    {
        await DoSuccessfulTest(["ContextValues", "CollectionItemIndexExpressions"], "JsonFile1.json");
    }
}