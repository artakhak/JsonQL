// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents a JSON value path, providing access to its components and associated line information.
/// </summary>
public interface IJsonValuePath
{
    /// <summary>
    /// Gets the line and position information associated with the JSON value path.
    /// </summary>
    /// <remarks>
    /// This property provides access to the <see cref="IJsonLineInfo"/> instance,
    /// which contains details about the line number and position of the JSON element
    /// described by this path, if available. Returns null if no line information is present.
    /// </remarks>
    IJsonLineInfo? LineInfo { get; }

    /// <summary>
    /// Gets the collection of elements that represent the components of the JSON value path.
    /// </summary>
    /// <remarks>
    /// This property provides access to the individual components of a JSON path, which are used
    /// to locate or resolve specific elements or values within a JSON structure. The path is
    /// represented as an ordered read-only list of <see cref="IJsonValuePathElement"/> instances,
    /// where each element corresponds to a segment of the path. For example, path elements may
    /// represent object properties or array indices.
    /// </remarks>
    IReadOnlyList<IJsonValuePathElement> Path { get; }
}

/// <inheritdoc />
public class JsonValuePath : IJsonValuePath
{
    /// <summary>
    /// Represents a path consisting of JSON value path elements, which can be used
    /// to navigate and locate specific data within a JSON structure.
    /// </summary>
    public JsonValuePath(List<IJsonValuePathElement> path)
    {
        Path = path;
    }

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo
    {
        get
        {
            if (Path.Count == 0)
                return null;

            return Path[0].LineInfo;
        }
    }

    /// <inheritdoc />
    public IReadOnlyList<IJsonValuePathElement> Path { get; }

    /// <inheritdoc />
    public override string ToString() => string.Join(JsonOperatorNames.JsonPathSeparator, Path);
}
