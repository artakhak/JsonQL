// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents an element of a JSON value path.
/// </summary>
/// <remarks>
/// This interface serves as the base representation for elements within a JSON path,
/// providing a mechanism for navigating or identifying specific components in a JSON structure.
/// Derived types may represent paths to object properties, array indexes, or other specialized selectors.
/// </remarks>
public interface IJsonValuePathElement
{
    /// <summary>
    /// Represents the line information associated with a JSON value path element.
    /// </summary>
    /// <remarks>
    /// This property provides information about the location of the JSON element within the source document,
    /// such as line or position details. It is primarily used for diagnostics and error reporting purposes.
    /// </remarks>
    IJsonLineInfo? LineInfo { get; }
}
