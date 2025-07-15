// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation;
using JsonQL.JsonObjects;

namespace JsonQL.Query;

/// <inheritdoc />
public class QueryManagerCompilationResultLogger : ICompilationResultLogger
{
    private readonly ICompilationResultLogger _compilationResultLogger;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="compilationResultLogger">Compilation result logger</param>
    public QueryManagerCompilationResultLogger(ICompilationResultLogger compilationResultLogger)
    {
        _compilationResultLogger = compilationResultLogger;
    }

    /// <inheritdoc />
    public void LogCompilationResult(IJsonTextData jsonTextData, ICompilationResult compilationResult)
    {
        IJsonTextData convertedJsonTextData = jsonTextData;
        ICompilationResult convertedCompilationResult = compilationResult;

        if (jsonTextData.TextIdentifier == Constants.QueryTextIdentifier &&
            compilationResult.CompilationErrors.Count > 0 &&
            compilationResult.CompilationErrors.Any(x => x.JsonTextIdentifier == Constants.QueryTextIdentifier))
        {
            // Query is transformed to "merge(parent.query)\n\r"
            // Lets extract the original query
            var jsonLines = jsonTextData.JsonText.Split(Environment.NewLine);

            foreach (var jsonLine in jsonLines)
            {
                if (jsonLine.StartsWith(Constants.QueryPrefix, StringComparison.Ordinal))
                {
                    convertedJsonTextData = new JsonTextData(Constants.QueryTextIdentifier,
                        jsonLine.Substring(Constants.QueryPrefix.Length,
                        jsonLine.Length - Constants.QueryPrefix.Length),
                        jsonTextData.ParentJsonTextData);

                    var compilationErrors = new List<ICompilationErrorItem>(compilationResult.CompilationErrors.Count);

                    foreach (var compilationError in compilationResult.CompilationErrors)
                    {
                        if (compilationError.JsonTextIdentifier == Constants.QueryTextIdentifier)
                        {
                            compilationErrors.Add(new CompilationErrorItem(compilationError.JsonTextIdentifier, compilationError.ErrorMessage,
                                compilationError.LineInfo == null ? null :
                                new JsonLineInfo(1, compilationError.LineInfo.LinePosition - Constants.QueryPrefix.Length)));
                        }
                        else
                        {
                            compilationErrors.Add(compilationError);
                        }
                    }

                    convertedCompilationResult = new CompilationResult(compilationResult.CompiledJsonFiles, compilationErrors);
                    break;
                }
            }
        }

        _compilationResultLogger.LogCompilationResult(convertedJsonTextData, convertedCompilationResult);
    }
}