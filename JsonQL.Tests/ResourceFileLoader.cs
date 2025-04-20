namespace JsonQL.Tests;

/// <summary>
/// A helper class for loading resource files.
/// </summary>
public static class ResourceFileLoader
{
    /// <summary>
    /// Returns text loaded from resource file
    /// </summary>
    /// <param name="resourcePath">Resource file path.</param>
    /// <exception cref="ArgumentException">Throws this exception.</exception>
    public static string LoadJsonFile(ResourcePath resourcePath)
    {
        var filePath = resourcePath.GetFilePath();

        using var stream = typeof(ResourceFileLoader).Assembly.GetManifestResourceStream(
            filePath);

        if (stream == null)
            throw new ArgumentException($"Failed to load json file [{filePath}]");

        using (stream)
        {
            using var streamReader = new StreamReader(stream);
            var commandText = streamReader.ReadToEnd();
            return commandText;
        }
    }
}