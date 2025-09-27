// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Represents a JSON path used to navigate data within a JSON structure.
/// </summary>
public interface IJsonPath
{
    /// <summary>
    /// If the value is not null, json text identifier for json file that has the json object that resulted in conversion error.
    /// </summary>
    string JsonTextIdentifier { get; }

    /// <summary>
    /// Gets the ordered collection of elements that constitute the JSON path,
    /// allowing navigation and identification of specific locations within a JSON structure.
    /// </summary>
    IReadOnlyList<IJsonPathElement> Path { get; }
}