// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Represents a JSON property name path element within a JSON path.
/// </summary>
public interface IJsonPropertyNamePathElement : IJsonPathElement
{
    /// <summary>
    /// Gets the name of the JSON property represented by this path element.
    /// </summary>
    string Name { get; }
}