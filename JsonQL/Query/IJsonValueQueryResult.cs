// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation;
using JsonQL.JsonObjects;

namespace JsonQL.Query;

/// <summary>
/// Represents the result of a JSON value query operation.
/// This interface defines the structure for holding the outcome of a query
/// that processes JSON data. It provides information about the parsed value
/// and any compilation errors encountered during the operation.
/// </summary>
public interface IJsonValueQueryResult
{
    IParsedValue? ParsedValue { get; }
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
}