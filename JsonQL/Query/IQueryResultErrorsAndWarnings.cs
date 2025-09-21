// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Query;

/// <summary>
/// Represents a mechanism to retrieve errors and warnings associated with
/// a query result during compilation and object conversion phases.
/// </summary>
public interface IQueryResultErrorsAndWarnings
{
    /// <summary>
    /// Gets a collection of compilation errors encountered during the query processing.
    /// This property provides detailed information about errors that occurred during
    /// the compilation phase. The collection is read-only and contains instances of
    /// types implementing the ICompilationErrorItem interface.
    /// </summary>
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }

    /// <summary>
    /// Gets a collection of errors encountered during the object conversion phase.
    /// This property provides detailed information about errors that occurred while
    /// converting JSON data into objects. The collection is managed by the IConversionErrors
    /// interface and includes methods to retrieve error details or filter errors by type.
    /// </summary>
    IConversionErrors ConversionErrors { get; }

    /// <summary>
    /// Gets a collection of warnings encountered during the object conversion phase.
    /// This property provides detailed information about non-critical issues or
    /// potential problems identified while converting the JSON data to the expected
    /// object type. The collection is read-only and contains instances of types
    /// implementing the IConversionError interface.
    /// </summary>
    IConversionErrors ConversionWarnings { get; }
}