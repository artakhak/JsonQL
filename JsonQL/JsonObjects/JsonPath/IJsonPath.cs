// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Text;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;

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

/// <inheritdoc />
public class JsonPath : IJsonPath
{
    private readonly string _pathToString;

    /// <summary>
    /// Represents a JSON path, which defines a sequence of navigation steps to a specific value or node within a JSON structure.
    /// This class implements transformations of path elements into a string representation for JSON path notation.
    /// </summary>
    public JsonPath(string jsonTextIdentifier, IReadOnlyList<IJsonPathElement> path)
    {
        JsonTextIdentifier = jsonTextIdentifier;
        Path = path;

        var pathToString = new StringBuilder();

        // IJsonPathElement have elements like a, b, [1, 2], which should be converted
        // to "a.b[1, 2]"

        var pathElementIndex = 0;

        while (pathElementIndex < path.Count)
        {
            var pathElement = path[pathElementIndex];

            if (pathElementIndex > 0)
                pathToString.Append(JsonOperatorNames.JsonPathSeparator);

            if (pathElement is IJsonPropertyNamePathElement propertyNamePathElement &&
                pathElementIndex < path.Count - 1 && path[pathElementIndex + 1] is IJsonArrayIndexesPathElement jsonArrayIndexesPathElement)
            {
                pathToString.Append(propertyNamePathElement);
                pathToString.Append(jsonArrayIndexesPathElement);

                // Skip the array indexes since we already added this.
                pathElementIndex += 2;
                continue;
            }
            
            pathToString.Append(pathElement);

            ++pathElementIndex;
        }

        pathToString.Append(", ").Append(nameof(JsonTextIdentifier)).Append(":").Append(this.JsonTextIdentifier);

        _pathToString = pathToString.ToString();
    }

    /// <inheritdoc />
    public string JsonTextIdentifier { get; }

    /// <inheritdoc />
    public IReadOnlyList<IJsonPathElement> Path { get; }

    /// <inheritdoc />
    public override string ToString() => _pathToString;
}
