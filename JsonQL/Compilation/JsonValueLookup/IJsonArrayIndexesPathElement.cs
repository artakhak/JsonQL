// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents a path element within a JSON value path that selects multiple indexes in a JSON array.
/// </summary>
public interface IJsonArrayIndexesPathElement : IJsonValuePathElement
{
    /// <summary>
    /// Gets a read-only collection of indexes representing positions within a JSON array.
    /// </summary>
    /// <remarks>
    /// This property provides access to indexes that can be used to navigate through or extract specific elements
    /// from a JSON array. It supports scenarios where multiple array positions are targeted as part of a JSON value path.
    /// </remarks>
    IReadOnlyList<IJsonArrayIndexInfo> ArrayIndexes { get; }
}
