namespace JsonQL.Tests;

public class TestJsonTextDataPath
{
    public TestJsonTextDataPath(IEnumerable<string> relativeFolderPath, string compiledFileName)
    {
        RelativeFolderPath = relativeFolderPath;
        CompiledFileName = compiledFileName;
        ParentFileName = null;
        ParentParentFileName = null;
    }

    public TestJsonTextDataPath(IEnumerable<string> relativeFolderPath, string compiledFileName, string parentFileName)
    {
        RelativeFolderPath = relativeFolderPath;
        CompiledFileName = compiledFileName;
        ParentFileName = parentFileName;
        ParentParentFileName = null;
    }

    public TestJsonTextDataPath(IEnumerable<string> relativeFolderPath, string compiledFileName, string parentFileName, string parentParentFileName)
    {
        RelativeFolderPath = relativeFolderPath;
        CompiledFileName = compiledFileName;
        ParentFileName = parentFileName;
        ParentParentFileName = parentParentFileName;
    }

    public IEnumerable<string> RelativeFolderPath { get; }
    public string CompiledFileName { get; }
    public string? ParentFileName { get; }
    public string? ParentParentFileName { get; }
}
