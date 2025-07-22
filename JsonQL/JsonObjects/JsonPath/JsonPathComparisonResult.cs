// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Indicates that the two JSON paths are identical.
/// </summary>
/// <remarks>
/// This value is returned when both JSON paths point to the same location within the JSON structure,
/// with no difference in hierarchy or structure.
/// </remarks>
public enum JsonPathComparisonResult
{
    /// <summary>
    /// Represents a state where the two JSON paths have no defined relationship.
    /// </summary>
    /// <remarks>
    /// The value <c>None</c> is used when comparing two JSON paths that do not share any direct or indirect
    /// hierarchical connection, and as such, are completely unrelated.
    /// </remarks>
    None,

    /// <summary>
    /// Represents a state where the first JSON path is a direct child of the second JSON path.
    /// </summary>
    /// <remarks>
    /// The value <c>Child</c> is used to indicate a hierarchical relationship where the first path
    /// originates directly under the second path in the JSON structure.
    /// </remarks>
    Child,

    /// <summary>
    /// Indicates that the first JSON path is a direct parent of the second JSON path.
    /// </summary>
    /// <remarks>
    /// This value is returned when the first path hierarchically contains the second path and the second path is
    /// an immediate descendant of the first. This relationship signifies a direct parent-child connection.
    /// </remarks>
    Parent,

    /// <summary>
    /// Represents a state where the two JSON paths are identical.
    /// </summary>
    /// <remarks>
    /// The value <c>Equal</c> is used when comparing two JSON paths that refer to the same
    /// location within the JSON structure, without any variation in hierarchy or structure.
    /// </remarks>
    Equal,

    /// <summary>
    /// Represents a state where the two JSON paths share the same parent but are distinct from each other.
    /// </summary>
    /// <remarks>
    /// The value <c>Sibling</c> is used when comparing two JSON paths that reside under the same parent node
    /// within the JSON structure, indicating that they are on the same hierarchical level without overlapping.
    /// </remarks>
    Sibling
}
