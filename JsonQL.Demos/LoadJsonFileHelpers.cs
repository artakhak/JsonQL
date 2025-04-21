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
}
