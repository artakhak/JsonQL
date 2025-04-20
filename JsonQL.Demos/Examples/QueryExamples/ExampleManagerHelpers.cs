using JsonQL.Diagnostics;
using System.Reflection;

namespace JsonQL.Demos.Examples.QueryExamples;

public static class ExampleManagerHelpers
{
    public static string GetResultFileName(Type exampleType) => $"{exampleType.Name}.result.json";
    public static string GetResultErrorsFileName(Type exampleType) => $"{exampleType.Name}.errors.json";

    public static string LoadExpectedResultJsonFile(IExampleManager exampleManager)
    {
        var exampleType = exampleManager.GetType();

        // Start from item 2 in namespace to exclude the first two namespace items JsonQL and Demos.
        // We normally would check if there are at least two items in exampleType.Namespace
        // but this is a demo application and not worth it.
        var resourceNamespace = exampleType.Namespace!.Split('.')[2..];
        return LoadJsonFileHelpers.LoadJsonFile(GetResultFileName(exampleType),
            resourceNamespace);
    }

    public static async Task SaveResultToApplicationOutputFolderAsync(IExampleManager exampleManager, string serializedResult)
    {
        await ResourceFileHelpers.SaveAsync(serializedResult, GetResultFileName(exampleManager.GetType()));
    }

    public static string GetOutputFilePath(IExampleManager exampleManager) =>
        Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, GetResultFileName(exampleManager.GetType()));
}