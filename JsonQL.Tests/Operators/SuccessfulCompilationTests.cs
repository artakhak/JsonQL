namespace JsonQL.Tests.Operators;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task Operators_TypeOfOperator()
    {
        await DoSuccessfulTest(["Operators", "TypeOfOperator"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task Operators_OperatorPriorities()
    {
        await DoSuccessfulTest(["Operators", "OperatorPriorities"], "JsonFile1.json");
    }

    [Test]
    public async Task Operators_IsNull()
    {
        await DoSuccessfulTest(["Operators", "IsNull"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task Operators_IsNotNull()
    {
        await DoSuccessfulTest(["Operators", "IsNotNull"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task Operators_IsUndefined()
    {
        await DoSuccessfulTest(["Operators", "IsUndefined"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task Operators_IsNotUndefined()
    {
        await DoSuccessfulTest(["Operators", "IsNotUndefined"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task Operators_DefaultValue()
    {
        await DoSuccessfulTest(["Operators", "DefaultValue"], "JsonFile1.json");
    }

    [Test]
    public async Task AssertHasValueTest()
    {
        await DoSuccessfulTest(["Operators", "AssertHasValue"], "JsonFile1.json");
    }
}
