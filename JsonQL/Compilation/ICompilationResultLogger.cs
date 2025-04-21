using System.Text;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation;

public interface ICompilationResultLogger
{
    void LogCompilationResult(IJsonTextData jsonTextData, ICompilationResult compilationResult);
}

public class CompilationResultLogger : ICompilationResultLogger
{
    /// <inheritdoc />
    public void LogCompilationResult(IJsonTextData jsonTextData, ICompilationResult compilationResult)
    {
        var compiledJsonData = compilationResult.CompiledJsonFiles.FirstOrDefault(x =>
            string.Equals(x.TextIdentifier, jsonTextData.TextIdentifier, StringComparison.OrdinalIgnoreCase));

        if (compiledJsonData != null && compilationResult.CompilationErrors.Count == 0)
        {
            LogHelper.Context.Log.InfoFormat("Successfully compiled json text with [{0}]=[{1}].",
                nameof(IJsonTextData.TextIdentifier), jsonTextData.TextIdentifier);
            return;
        }

        var jsonTextIdentifierToJsonTextData = new Dictionary<string, JsonTextDataWithJsonLines>(StringComparer.OrdinalIgnoreCase);

        foreach (var errorsGroupedByFileIdentifier in compilationResult.CompilationErrors.GroupBy(x =>
                         x.JsonTextIdentifier)
                     .Where(x => x.Any()))
        {
            var currentJsonTextIdentifier = errorsGroupedByFileIdentifier.Key;

            var errorsStringBuilder = new StringBuilder();

            errorsStringBuilder.AppendLine();
            errorsStringBuilder.AppendFormat("Compilation errors for json text with [{0}]=[{1}].",
                nameof(ICompilationErrorItem.JsonTextIdentifier), currentJsonTextIdentifier);

            errorsStringBuilder.AppendLine();

            foreach (var compilationErrorByLineNumber in
                     errorsGroupedByFileIdentifier.GroupBy(x => x.LineInfo?.LineNumber ?? -1)
                         .OrderBy(x => x.Key))
            {
                string? trimmedErrorLine = null;

                foreach (var compilationError in compilationErrorByLineNumber)
                {
                    errorsStringBuilder.Append("Compilation error: ");

                    if (compilationError.LineInfo != null)
                    {
                        errorsStringBuilder.AppendFormat("[{0}]:[{1}], [{2}]:[{3}],",
                            nameof(IJsonLineInfo.LineNumber), compilationError.LineInfo.LineNumber,
                            nameof(IJsonLineInfo.LinePosition), compilationError.LineInfo.LinePosition);
                    }

                    errorsStringBuilder.AppendFormat("[{0}]:[{1}].", nameof(ICompilationErrorItem.ErrorMessage), compilationError.ErrorMessage);

                    errorsStringBuilder.AppendLine();

                    if (compilationError.LineInfo != null)
                    {
                        if (!jsonTextIdentifierToJsonTextData.TryGetValue(currentJsonTextIdentifier, out var jsonTextDataWithJsonLines))
                        {
                            jsonTextDataWithJsonLines = string.Equals(jsonTextData.TextIdentifier, currentJsonTextIdentifier, StringComparison.OrdinalIgnoreCase) ?
                                new JsonTextDataWithJsonLines(jsonTextData) :
                                GetParentJsonTextData(jsonTextData, currentJsonTextIdentifier);

                            if (jsonTextDataWithJsonLines != null)
                                jsonTextIdentifierToJsonTextData[currentJsonTextIdentifier] = jsonTextDataWithJsonLines;
                        }

                        if (jsonTextDataWithJsonLines != null)
                        {
                            if (compilationError.LineInfo.LineNumber >= 1 && compilationError.LineInfo.LineNumber <= jsonTextDataWithJsonLines.JsonLines.Count)
                            {
                                var errorLine = jsonTextDataWithJsonLines.JsonLines[compilationError.LineInfo.LineNumber - 1];

                                if (compilationError.LineInfo.LinePosition >= 1 && compilationError.LineInfo.LinePosition <= errorLine.Length)
                                {
                                    trimmedErrorLine ??= errorLine.TrimStart();

                                    errorsStringBuilder.Append('\t');

                                    var errorLinePositionInTrimmedLine = compilationError.LineInfo.LinePosition - 1 - (errorLine.Length - trimmedErrorLine.Length);

                                    trimmedErrorLine = trimmedErrorLine.TrimEnd();
                                    const int maxErrorLineHalfLength = 50;

                                    if (errorLinePositionInTrimmedLine >= maxErrorLineHalfLength)
                                    {
                                        var startIndex = errorLinePositionInTrimmedLine - maxErrorLineHalfLength;
                                        errorLinePositionInTrimmedLine = maxErrorLineHalfLength;

                                        trimmedErrorLine = trimmedErrorLine.Substring(startIndex, 
                                            Math.Min(trimmedErrorLine.Length - startIndex, maxErrorLineHalfLength << 1));
                                    }
                                    else
                                    {
                                        trimmedErrorLine = trimmedErrorLine.Substring(0,
                                            Math.Min(trimmedErrorLine.Length, maxErrorLineHalfLength << 1));
                                    }

                                    errorsStringBuilder.AppendLine(trimmedErrorLine);

                                    errorsStringBuilder.Append('\t');

                                    if (errorLinePositionInTrimmedLine > 0)
                                    {
                                        errorsStringBuilder.Append(new string(' ', errorLinePositionInTrimmedLine));
                                    }

                                    // Up arrow character does not show in Log4Net console logs in demo app.
                                    // Unicode or Ascii log4net appender should be used to see this character.
                                    //errorsStringBuilder.Append('↑');
                                    errorsStringBuilder.Append('\u2191');
                                    errorsStringBuilder.Append(new string('-', 30));
                                }
                                else
                                {
                                    // If we get here, this is a bug and should be fixed
                                    errorsStringBuilder.AppendFormat("Invalid error line position [{0}]. Expected to be a number between [1] and [{1}]. THIS IS A BUG.",
                                        compilationError.LineInfo.LinePosition, errorLine.Length);
                                }
                            }
                            else
                            {
                                errorsStringBuilder.AppendFormat("Invalid error line number [{0}]. Expected to be a number between [1] and [{1}]. THIS IS A BUG.",
                                    compilationError.LineInfo.LineNumber, jsonTextDataWithJsonLines.JsonLines.Count);
                            }
                        }
                        else
                        {
                            errorsStringBuilder.AppendFormat("Json text with identifier=[{0}] not found.", currentJsonTextIdentifier);
                        }

                        errorsStringBuilder.AppendLine();
                    }
                }
            }

            LogHelper.Context.Log.Error(errorsStringBuilder.ToString());
        }
    }

    private JsonTextDataWithJsonLines? GetParentJsonTextData(IJsonTextData compiledJsonTextData, string parentTextIdentifier)
    {
        IJsonTextData? parentJsonTextData = compiledJsonTextData.ParentJsonTextData;

        while (parentJsonTextData != null)
        {
            if (string.Equals(parentJsonTextData.TextIdentifier, parentTextIdentifier, StringComparison.OrdinalIgnoreCase))
                return new JsonTextDataWithJsonLines(parentJsonTextData);

            parentJsonTextData = parentJsonTextData.ParentJsonTextData;
        }

        return null;
    }
}