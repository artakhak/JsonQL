using System.Text;
using JsonQL.Compilation;
using JsonQL.Query;

namespace JsonQL.Diagnostics.ResultValidation;

public static class JsonQLResultValidator
{
    public const string SerializedFileName = "QueryResult.json";

    public static async Task ValidateResultAsync(JsonQLResultValidationParameters jsonQlResultValidationParameters)
    {
        var result = await jsonQlResultValidationParameters.GetJsonQlResultAsync();

        var serializedResult = RemoveLineEndSpaces(SerializeResult(result));
       
        await SaveResultToApplicationOutputFolderAsync(serializedResult);
        var expectedJsonFile = RemoveLineEndSpaces(await jsonQlResultValidationParameters.LoadExpectedResultJsonFileAsync());

        if (!string.Equals(serializedResult, expectedJsonFile, StringComparison.Ordinal))
            throw new JsonQLResultValidationException("The contents of expected result and actual result do not match.");
    }

    /// <summary>
    /// Editors like IntelliJ remove line end spaces and the validation with compile query result differs form
    /// expected one. Lets remove the empty spaces at the end of line
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static string RemoveLineEndSpaces(string text)
    {
        var lines = text.Split(Environment.NewLine);
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

    private static string SerializeResult(object result)
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
        await ResourceFileHelpers.SaveAsync(serializedResult, SerializedFileName);
    }
}
