using JsonQL.Compilation;
using JsonQL.Diagnostics;
using JsonQL.Query;
using OROptimizer.Diagnostics.Log;
using System.Reflection;
using System.Text;

namespace JsonQL.Demos.Examples;

public abstract class ExampleManagerAbstr : IExampleManager
{
    /// <inheritdoc />
    public abstract bool IsSuccessfulEvaluationExample { get; }

    /// <inheritdoc />
    public async Task ExecuteAsync()
    {
        LogHelper.Context.Log.InfoFormat("------EXECUTING EXAMPLE [{0}]---------------------------", this.GetType().FullName!);
        if (!this.IsSuccessfulEvaluationExample)
            LogHelper.Context.Log.InfoFormat("------------NOTE: THIS IS FAILURE TEST AND ERROR LOGS ARE EXPECTED!!!----------------------");

        var result = await GetJsonQlResultAsync();

        var serializedResult = RemoveLineEndSpaces(SerializeResult(result));

        await ResourceFileHelpers.SaveAsync(serializedResult, "QueryResult.json");

        await SaveResultToApplicationOutputFolderAsync(serializedResult);
        var expectedJsonFile = RemoveLineEndSpaces(this.LoadExpectedResultJsonFile());

        if (!string.Equals(serializedResult, expectedJsonFile, StringComparison.Ordinal))
        {
            LogHelper.Context.Log.ErrorFormat("FAILURE: Example [{0}]---------------------------", this.GetType().FullName!);
            throw new ApplicationException($"The contents of expected result and actual result do not match. Example type: [{GetType()}].");
        }
        else
        {
            LogHelper.Context.Log.InfoFormat("SUCCESS: Example [{0}]---------------------------", this.GetType().FullName!);
        }

        LogHelper.Context.Log.InfoFormat("-----------------------------------------");
        LogHelper.Context.Log.InfoFormat("Generated output is in file [{0}].", GetOutputFilePath());
        LogHelper.Context.Log.InfoFormat("-----------------------------------------");
    }

    /// <summary>
    /// Editors like IntelliJ remove line end spaces and the validation with compile query result differs form
    /// expected one. Lets remove the empty spaces at the end of line
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string RemoveLineEndSpaces(string text)
    {
        var lines = text.Split(System.Environment.NewLine);
        var result = new StringBuilder(text.Length);

        foreach (var line in lines)
        {
            var trimmedLine = line.TrimEnd();

            if (trimmedLine.Length == 0)
                continue;

            if (result.Length > 0)
                result.AppendLine();

            result.Append(trimmedLine);
        }

        return result.ToString();
    }

    protected abstract Task<object> GetJsonQlResultAsync();

    protected string SerializeResult(object result)
    {
        if (result is ICompilationResult compilationResult)
        {
            return CompilationResultSerializerAmbientContext.Context.Serialize(compilationResult,
                x =>
                    // Lets output only the most recently compiled file
                    x.TextIdentifier == compilationResult.CompiledJsonFiles[^1].TextIdentifier);
        }

        if (result is IJsonValueQueryResult jsonValueQueryResult)
        {
            return CompilationResultSerializerAmbientContext.Context.Serialize(jsonValueQueryResult);
        }

        if (result is IObjectQueryResult objectQueryResult)
        {
            return CompilationResultSerializerAmbientContext.Context.Serialize(objectQueryResult);
        }

        throw new ApplicationException($"No conversion exists for type [{result.GetType()}].");
    }

    private static async Task SaveResultToApplicationOutputFolderAsync(string serializedResult)
    {
        await ResourceFileHelpers.SaveAsync(serializedResult, "QueryResult.json");
    }

    public static string GetOutputFilePath() =>
        Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "QueryResult.json");
}