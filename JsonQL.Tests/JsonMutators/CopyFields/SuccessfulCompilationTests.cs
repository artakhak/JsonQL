namespace JsonQL.Tests.JsonMutators.CopyFields;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task CopyFieldsFromParentTest()
    {
        await DoSuccessfulTest(["JsonMutators", "CopyFields", "CopyFieldsFromParent"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task CopyFieldsFromCurrentJsonTest()
    {
        await DoSuccessfulTest(["JsonMutators", "CopyFields", "CopyFieldsFromCurrentJson"], "JsonFile1.json");
    }

    [Test]
    public async Task CopiedFieldsReplaceObjectFieldsTest()
    {
        await DoSuccessfulTest(["JsonMutators", "CopyFields", "CopiedFieldsReplaceObjectFields"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task CopiedFieldsReplacedByObjectFieldsTest()
    {
        await DoSuccessfulTest(["JsonMutators", "CopyFields", "CopiedFieldsReplacedByObjectFields"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task CopyFieldsOfObjectSelectedWithArrayIndexersTest()
    {
        await DoSuccessfulTest(["JsonMutators", "CopyFields", "CopyFieldsOfObjectSelectedWithArrayIndexers"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task CopyFieldsOfObjectSelectedWithFilteringTest()
    {
        await DoSuccessfulTest(["JsonMutators", "CopyFields", "CopyFieldsOfObjectSelectedWithFiltering"], "JsonFile2.json", "JsonFile1.json");
    }
}