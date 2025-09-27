// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Represents an interface for JSON path elements that specify array indexes.
/// Provides a contract to access the list of indexes targeting specific elements within a JSON array.
/// </summary>
public interface IJsonArrayIndexesPathElement : IJsonPathElement
{
    /// <summary>
    /// Gets the list of indexes in a JSON array path element.
    /// Provides access to the specific array positions targeted by this path element.
    /// </summary>
    IReadOnlyList<int> Indexes { get; }
}