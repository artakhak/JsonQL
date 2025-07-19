using JsonQL.Compilation;

namespace JsonQL.Tests;

public class JsonTextDataLoader
{
    //private readonly string _folderName;
    private readonly List<string> _folderNames = new(5);

    /// <summary>
    ///
    /// </summary>
    /// <param name="folderName"> Folder name inside "JsonFiles" folder. Example "SimpleValueReferences" which is under "JsonFiles".</param>
    public JsonTextDataLoader(string folderName)
    {
        _folderNames.Add(folderName);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="folderNames">
    /// List of folder names that specifies path relative to "JsonFiles" folder of the folder containing tested json files.
    /// For example if <param name="folderNames"></param> is ["SimpleValueReferences", "Examples1"], then the files will
    /// be in folder "JsonFiles/SimpleValueReferences/SimpleExamples".
    /// </param>
    public JsonTextDataLoader(IEnumerable<string> folderNames)
    {
        _folderNames.AddRange(folderNames);
    }

    public IJsonTextData GetJsonTextData(string compiledFileName, string? parentFileName = null, string? parentParentFileName = null)
    {
        if (parentParentFileName == null)
        {
            if (parentFileName == null)
                return GetJsonTextDataWithNoParent(compiledFileName);

            return GetJsonTextDataWithParent(compiledFileName, parentFileName);
        }

        Assert.That(parentFileName, Is.Not.Null);
        return GetJsonTextDataWithTwoLevelParents(compiledFileName, parentFileName!, parentParentFileName);
    }

    private IJsonTextData GetJsonTextDataWithNoParent(string compiledFileName)
    {
        return LoadJsonTextData(compiledFileName, compiledFileName);
    }

    private IJsonTextData GetJsonTextDataWithParent(string compiledFileName, string parentFileName)
    {
        return new JsonTextData(compiledFileName, ResourceFileLoader.LoadJsonFile(new ResourcePath(compiledFileName, _folderNames)),
            LoadJsonTextData(parentFileName, parentFileName));
    }

    private IJsonTextData GetJsonTextDataWithTwoLevelParents(string compiledFileName, string parentFileName, string parentParentFileName)
    {
        return new JsonTextData(compiledFileName, ResourceFileLoader.LoadJsonFile(new ResourcePath(compiledFileName, _folderNames)),
            LoadJsonTextData(parentFileName, parentFileName,
                LoadJsonTextData(parentParentFileName, parentParentFileName)));
    }

    private IJsonTextData LoadJsonTextData(string fileIdentifier, string fileName, IJsonTextData? parenTextData = null)
    {
        return new JsonTextData(fileIdentifier, ResourceFileLoader.LoadJsonFile(new ResourcePath(fileName, _folderNames)), parenTextData);
    }
}