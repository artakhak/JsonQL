namespace JsonQL.Demos.Examples;

public interface IExampleManager
{
    bool IsSuccessfulEvaluationExample { get; }
    Task ExecuteAsync();
}

/// <summary>
/// Extensions for 
/// </summary>
public static class ExampleManagerExtensions
{
    /// <summary>
    /// Loads a JSON file associated with the example manager's namespace.
    /// </summary>
    /// <param name="exampleManager">The example manager used to determine the namespace.</param>
    /// <param name="jsonFileName">The name of the JSON file to load.</param>
    /// <returns>Returns the content of the loaded JSON file as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when the JSON file cannot be loaded.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the type of <paramref name="exampleManager"/> or its namespace is null.</exception>
    public static string LoadExampleJsonFile(this IExampleManager exampleManager, string jsonFileName)
    {
        return LoadJsonFileHelpers.LoadExampleJsonFile(jsonFileName, exampleManager.GetType());
    }

    /// <summary>
    /// Loads the expected result JSON file associated with the example manager's namespace.
    /// </summary>
    /// <param name="exampleManager">The example manager used to determine the namespace and locate the JSON file.</param>
    /// <returns>Returns the content of the loaded expected result JSON file as a string.</returns>
    /// <exception cref="ArgumentException">Thrown when the JSON file cannot be loaded.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the type of <paramref name="exampleManager"/> or its namespace is null.</exception>
    public static string LoadExpectedResultJsonFile(this IExampleManager exampleManager)
    {
        return LoadJsonFileHelpers.LoadExampleJsonFile("Result.json", exampleManager.GetType());
    }
}