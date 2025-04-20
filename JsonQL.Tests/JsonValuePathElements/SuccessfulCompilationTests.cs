namespace JsonQL.Tests.JsonValuePathElements;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task JsonValuePathElements_Where()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "Where"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_Reverse()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "Reverse"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_Flatten()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "Flatten"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_Select()
    {
        await DoSuccessfulTest(new ResourcePath("JsonFile2.json", ["JsonValuePathElements", "Select"]),
            new ResourcePath("JsonFile1.json", ["JsonValuePathElements", "Select"]),
            new ResourcePath("CompaniesAsArrayInJson.json", ["SharedJsonFiles"]));
    }

    [Test]
    public async Task JsonValuePathElements_ArrayIndexers()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "ArrayIndexers"], "JsonFile2.json", "JsonFile1.json");
    }
    [Test]
    public async Task JsonValuePathElements_At()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "At"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_First()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "First"], "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_Last()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "Last"], "JsonFile2.json", "JsonFile1.json");
    }
}