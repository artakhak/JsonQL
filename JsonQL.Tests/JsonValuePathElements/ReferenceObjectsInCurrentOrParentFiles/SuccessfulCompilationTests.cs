namespace JsonQL.Tests.JsonValuePathElements.ReferenceObjectsInCurrentOrParentFiles;

[TestFixture]
public class SuccessfulCompilationTests : SuccessfulJsonCompilationTestsAbstr
{
    [Test]
    public async Task JsonValuePathElements_ReferenceObjectsInCurrentOrParentFiles_this_and_parent()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "ReferenceObjectsInCurrentOrParentFiles", "this_and_parent"], "JsonFile3.json", "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_ReferenceObjectsInCurrentOrParentFiles_ReferenceParentRootJson()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "ReferenceObjectsInCurrentOrParentFiles", "ReferenceParentRootJson"], 
            "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_ReferenceObjectsInCurrentOrParentFiles_ReferenceParentRootArray()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "ReferenceObjectsInCurrentOrParentFiles", "ReferenceParentRootArray"],
            "JsonFile2.json", "JsonFile1.json");
    }

    [Test]
    public async Task JsonValuePathElements_ReferenceObjectsInCurrentOrParentFiles_ReferenceCurrentFileRootArray()
    {
        await DoSuccessfulTest(["JsonValuePathElements", "ReferenceObjectsInCurrentOrParentFiles", "ReferenceCurrentFileRootArray"],
            "JsonFile2.json", "JsonFile1.json");
    }
}