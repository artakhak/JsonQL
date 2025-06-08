using JsonQL.JsonObjects;

namespace JsonQL.Tests.JsonValuePathTests;

[TestFixture]
public class JsonValuePathTests: JsonCompilationTestsAbstr
{
    /// <summary>
    /// Tests fo rjson value path when json values are not copied
    /// </summary>
    [Test]
    public void OriginalJsonValuePathTests()
    {
        var jsonTextDataLoader = new JsonTextDataLoader(["JsonValuePathTests", "CopiedJsonValueTests"]);

        var jsonTextData = jsonTextDataLoader
            .GetJsonTextData("JsonFile2.json", "JsonFile1.json");

        var compilationResult = JsonCompiler.Compile(jsonTextData);

        Assert.That(compilationResult.CompilationErrors.Count, Is.EqualTo(0));

        var compiledJsonData = compilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == JsonFileIdentifiers.JsonFile1);

        Assert.That(compiledJsonData.CompiledParsedValue, Is.InstanceOf(typeof(IRootParsedJson)));
        
        var employeesJson = ((IRootParsedJson)compiledJsonData.CompiledParsedValue)["Employees"].Value as IParsedArrayValue;

        Assert.That(employeesJson, Is.Not.Null);

        Assert.That(employeesJson.GetPath().ToString(), Is.EqualTo("Root.Employees, JsonTextIdentifier:JsonFile1"));
        Assert.That(employeesJson.PathInReferencedJson, Is.Null);

        var secondEmployeeJson = employeesJson.Values[1] as IParsedJson;
        Assert.That(secondEmployeeJson, Is.Not.Null);

        Assert.That(secondEmployeeJson.GetPath().ToString(), Is.EqualTo("Root.Employees[1], JsonTextIdentifier:JsonFile1"));
        Assert.That(secondEmployeeJson.PathInReferencedJson, Is.Null);

        Assert.That(secondEmployeeJson["Id"].Value.GetPath().ToString(), Is.EqualTo("Root.Employees[1].Id, JsonTextIdentifier:JsonFile1"));
        Assert.That(secondEmployeeJson["Id"].Value.PathInReferencedJson, Is.Null);
        
        var multiDimensionalJsonArray = ((IRootParsedJson)compiledJsonData.CompiledParsedValue)["MultiDimensionalArray"].Value as IParsedArrayValue;
        Assert.That(multiDimensionalJsonArray, Is.Not.Null);

        Assert.That(multiDimensionalJsonArray.Values[2].GetPath().ToString(), 
            Is.EqualTo("Root.MultiDimensionalArray[2], JsonTextIdentifier:JsonFile1"));
        Assert.That(multiDimensionalJsonArray.Values[2].PathInReferencedJson, Is.Null);

        Assert.That(((IParsedArrayValue)multiDimensionalJsonArray.Values[2]).Values[1].GetPath().ToString(),
            Is.EqualTo("Root.MultiDimensionalArray[2,1], JsonTextIdentifier:JsonFile1"));
        Assert.That(((IParsedArrayValue)multiDimensionalJsonArray.Values[2]).Values[1].PathInReferencedJson, Is.Null);

        Assert.That(((IParsedJson)((IParsedArrayValue)multiDimensionalJsonArray.Values[3]).Values[1])["Id"].Value.GetPath().ToString(),
            Is.EqualTo("Root.MultiDimensionalArray[3,1].Id, JsonTextIdentifier:JsonFile1"));
        Assert.That(((IParsedJson)((IParsedArrayValue)multiDimensionalJsonArray.Values[3]).Values[1])["Id"].Value.PathInReferencedJson, Is.Null);
    }

    [Test]
    public void CopiedJsonValuePathTests()
    {
        var jsonTextDataLoader = new JsonTextDataLoader(["JsonValuePathTests", "CopiedJsonValueTests"]);

        var jsonTextData = jsonTextDataLoader
            .GetJsonTextData("JsonFile2.json", "JsonFile1.json");

        var compilationResult = JsonCompiler.Compile(jsonTextData);

        Assert.That(compilationResult.CompilationErrors.Count, Is.EqualTo(0));

        var compiledJsonData = compilationResult.CompiledJsonFiles.First(x => x.TextIdentifier == JsonFileIdentifiers.JsonFile2);

        Assert.That(compiledJsonData.CompiledParsedValue, Is.InstanceOf(typeof(IRootParsedJson)));

        var employeesJsonCopiedFromParent = ((IRootParsedJson)compiledJsonData.CompiledParsedValue)["EmployeesCopiedFromParentJson"].Value as IParsedArrayValue;

        Assert.That(employeesJsonCopiedFromParent, Is.Not.Null);

        Assert.That(employeesJsonCopiedFromParent.GetPath().ToString(), Is.EqualTo("Root.EmployeesCopiedFromParentJson, JsonTextIdentifier:JsonFile2"));
        Assert.That(employeesJsonCopiedFromParent.PathInReferencedJson!.ToString(), Is.EqualTo("Root.Employees, JsonTextIdentifier:JsonFile1"));
       
        var secondCopiedEmployeeJson = employeesJsonCopiedFromParent.Values[1] as IParsedJson;
        Assert.That(secondCopiedEmployeeJson, Is.Not.Null);

        Assert.That(secondCopiedEmployeeJson.GetPath().ToString(), Is.EqualTo("Root.EmployeesCopiedFromParentJson[1], JsonTextIdentifier:JsonFile2"));
        Assert.That(secondCopiedEmployeeJson.PathInReferencedJson!.ToString(), Is.EqualTo("Root.Employees[1], JsonTextIdentifier:JsonFile1"));

        Assert.That(secondCopiedEmployeeJson["Id"].Value.GetPath().ToString(), Is.EqualTo("Root.EmployeesCopiedFromParentJson[1].Id, JsonTextIdentifier:JsonFile2"));
        Assert.That(secondCopiedEmployeeJson["Id"].Value.PathInReferencedJson!.ToString(), Is.EqualTo("Root.Employees[1].Id, JsonTextIdentifier:JsonFile1"));

        var copiedMultiDimensionalJsonArray = ((IRootParsedJson)compiledJsonData.CompiledParsedValue)["MultiDimensionalArrayCopiedFromParentJson"].Value as IParsedArrayValue;
        Assert.That(copiedMultiDimensionalJsonArray, Is.Not.Null);

        Assert.That(copiedMultiDimensionalJsonArray.Values[2].GetPath().ToString(),
            Is.EqualTo("Root.MultiDimensionalArrayCopiedFromParentJson[2], JsonTextIdentifier:JsonFile2"));
        Assert.That(copiedMultiDimensionalJsonArray.Values[2].PathInReferencedJson!.ToString(),
            Is.EqualTo("Root.MultiDimensionalArray[2], JsonTextIdentifier:JsonFile1"));

        Assert.That(((IParsedArrayValue)copiedMultiDimensionalJsonArray.Values[2]).Values[1].GetPath().ToString(),
            Is.EqualTo("Root.MultiDimensionalArrayCopiedFromParentJson[2,1], JsonTextIdentifier:JsonFile2"));
        Assert.That(((IParsedArrayValue)copiedMultiDimensionalJsonArray.Values[2]).Values[1].PathInReferencedJson!.ToString(), 
            Is.EqualTo("Root.MultiDimensionalArray[2,1], JsonTextIdentifier:JsonFile1"));

        Assert.That(((IParsedJson)((IParsedArrayValue)copiedMultiDimensionalJsonArray.Values[3]).Values[1])["Id"].Value.GetPath().ToString(),
            Is.EqualTo("Root.MultiDimensionalArrayCopiedFromParentJson[3,1].Id, JsonTextIdentifier:JsonFile2"));
        Assert.That(((IParsedJson)((IParsedArrayValue)copiedMultiDimensionalJsonArray.Values[3]).Values[1])["Id"].Value.PathInReferencedJson!.ToString(), 
            Is.EqualTo("Root.MultiDimensionalArray[3,1].Id, JsonTextIdentifier:JsonFile1"));
    }
}