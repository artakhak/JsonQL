namespace JsonQL.Tests.Functions.StringFunctions;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task Len_Function_TextLengthTest()
    {
        await DoSuccessfulTest(["Functions", "StringFunctions", "Len"], "JsonFile1.json");
    }

    [Test]
    public async Task Lower_Function_Test()
    {
        await DoSuccessfulTest(["Functions", "StringFunctions", "Lower"], "JsonFile1.json");
    }

    [Test]
    public async Task Upper_Function_Test()
    {
        await DoSuccessfulTest(["Functions", "StringFunctions", "Upper"], "JsonFile1.json");
    }

    [Test]
    public async Task Concatenate_Function_Test()
    {
        await DoSuccessfulTest(["Functions", "StringFunctions", "Concatenate"], "JsonFile1.json");
    }
}
