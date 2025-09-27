// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation;

/// <summary>
/// Represents the result of a JSON compilation process.
/// </summary>
public interface ICompilationResult
{
    /// <summary>
    /// Represents a collection of errors encountered during the JSON compilation process.
    /// If this list is empty, it indicates a successful compilation. Otherwise, it contains
    /// one or more items describing the issues that occurred.
    /// </summary>
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }

    /// <summary>
    /// List of compiled JSON files. If <see cref="CompilationErrors"/> is empty, the list is not empty.
    /// Otherwise, the list might be empty or might have some items (for example, if some parent JSON files were compiled, but the main
    /// file failed to compile).
    /// </summary>
    IReadOnlyList<ICompiledJsonData> CompiledJsonFiles { get; }
}