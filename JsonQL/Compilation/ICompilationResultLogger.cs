// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.Compilation;

/// <summary>
/// Represents a logger interface for handling the results of a JSON compilation process.
/// </summary>
public interface ICompilationResultLogger
{
    /// <summary>
    /// Logs the result of a JSON compilation process.
    /// </summary>
    /// <param name="jsonTextData">
    /// The JSON text data containing information about the JSON text being compiled.
    /// </param>
    /// <param name="compilationResult">
    /// The result of the compilation, including any errors and the compiled JSON files.
    /// </param>
    void LogCompilationResult(IJsonTextData jsonTextData, ICompilationResult compilationResult);
}