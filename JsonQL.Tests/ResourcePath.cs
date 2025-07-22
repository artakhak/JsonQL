namespace JsonQL.Tests;

/// <summary>
/// Resource path data.
/// </summary>
/// <param name="ResourceFileName">Resource file name. Example "JsonFile1.json"</param>
/// <param name="PathFolderNames">Absolute path folders list. Example ["JsonValuePathElements", "Select"].</param>
public record ResourcePath(string ResourceFileName, IReadOnlyList<string> PathFolderNames);

public static class ResourcePathExtensions
{
    /// <summary>
    /// Resource file path.
    /// </summary>
    /// <param name="resourcePath"></param>
    /// <returns></returns>
    public static string GetFilePath(this ResourcePath resourcePath)
    {
        return string.Concat(typeof(ResourcePathExtensions).Namespace, '.', string.Join('.', resourcePath.PathFolderNames), '.', resourcePath.ResourceFileName);
    }
}
