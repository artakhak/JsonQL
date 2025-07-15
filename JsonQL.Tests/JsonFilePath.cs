namespace JsonQL.Tests;

public class JsonFilePath
{
    public JsonFilePath(string jsonFileName, IEnumerable<string> relativeFolderPath)
    {
        JsonFileName = jsonFileName;
        RelativeFolderPath = relativeFolderPath;
    }

    public string JsonFileName { get; }
    public IEnumerable<string> RelativeFolderPath { get; }
}