namespace JsonQL.Demos;

public static class LoadJsonFileHelpers
{
    /// <summary>
    /// Loads json text from a resource in namespace "JsonQL.Demos" located in folder specified by <paramref name="relativeFolderPath"/><br/>
    /// and file <paramref name="jsonFileName"/>.<br/>
    /// Example for value of the parameters is:<br/>
    /// <paramref name="jsonFileName"/>: "Companies.json"<br/>
    /// <paramref name="relativeFolderPath"/>: ["Demos", "QueryExamples"]
    /// </summary>
    /// <param name="jsonFileName">Json file name.</param>
    /// <param name="relativeFolderPath">File path.</param>
    /// <returns>Returns loaded json text.</returns>
    /// <exception cref="ArgumentException">Throws this exception.</exception>
    public static string LoadJsonFile(string jsonFileName, IEnumerable<string> relativeFolderPath)
    {
        using var stream = typeof(LoadJsonFileHelpers).Assembly.GetManifestResourceStream(
            string.Concat("JsonQL.Demos", '.', string.Join('.', relativeFolderPath), '.', jsonFileName));

        if (stream == null)
            throw new ArgumentException($"Failed to load json file [{jsonFileName}]");

        using (stream)
        {
            using var streamReader = new StreamReader(stream);
            var commandText = streamReader.ReadToEnd();
            return commandText;
        }
    }

    /// <summary>
    /// Loads JSON text from a resource in namespace "JsonQL.Demos" located in a folder derived from the namespace
    /// of <paramref name="exampleType"/><br/>
    /// and file <paramref name="jsonFileName"/>.
    /// </summary>
    /// <param name="jsonFileName">JSON file name.</param>
    /// <param name="exampleType">Type that determines the relative folder path based on its namespace.</param>
    /// <returns>Returns loaded JSON text.</returns>
    /// <exception cref="ArgumentException">Thrown when the JSON file cannot be loaded.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exampleType"/> or its namespace is null.</exception>
    public static string LoadExampleJsonFile(string jsonFileName, Type exampleType)
    {
        return LoadJsonFile(jsonFileName, GetExampleFolderRelativePath(exampleType));
    }

    /// <summary>
    /// Computes the folder path relative to the "JsonQL.Demos" namespace for the given <paramref name="exampleType"/>.<br/>
    /// The returned path excludes the "JsonQL.Demos" portion of the namespace.
    /// </summary>
    /// <param name="exampleType">The type whose namespace will be processed to compute the folder relative path.</param>
    /// <returns>Returns a list of strings representing the relative folder path.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="exampleType"/> or its namespace is null.</exception>
    public static IReadOnlyList<string> GetExampleFolderRelativePath(Type exampleType)
    {
        // Exclude "JsonQL.Demos" from namespaces.
        // For example, if type is JsonQL.Demos.Examples.IQueryManagerExamples.Example1
        // then the returned path will be ["Examples", "IQueryManagerExamples"]
        return exampleType.Namespace!.Split(".")[2..];
    }
}
