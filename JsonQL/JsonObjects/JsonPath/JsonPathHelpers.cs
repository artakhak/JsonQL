namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Provides helper methods for comparing and analyzing JSON paths.
/// </summary>
public static class JsonPathHelpers
{
    /// <summary>
    /// Compares two JSON paths and determines their relationship.
    /// </summary>
    /// <param name="jsonPath1">The first JSON path to compare.</param>
    /// <param name="jsonPath2">The second JSON path to compare.</param>
    /// <returns>
    /// A <see cref="JsonPathComparisonResult"/> value that represents the relationship
    /// between the two JSON paths. Possible results include:
    /// <list type="bullet">
    /// <item><description><c>None</c>: The paths have no relationship.</description></item>
    /// <item><description><c>Child</c>: The first path is a child of the second.</description></item>
    /// <item><description><c>Parent</c>: The first path is a parent of the second.</description></item>
    /// <item><description><c>Equal</c>: The paths are equivalent.</description></item>
    /// <item><description><c>Sibling</c>: The paths are siblings.</description></item>
    /// </list>
    /// </returns>
    public static JsonPathComparisonResult Compare(IJsonPath jsonPath1, IJsonPath jsonPath2)
    {
        if (jsonPath1.Path.Count == 0 || jsonPath2.Path.Count == 0)
            return JsonPathComparisonResult.None;

        if (jsonPath1.Path.Count > jsonPath2.Path.Count)
            return IsChildOfOrSameAs(jsonPath1, jsonPath2, out _) ? JsonPathComparisonResult.None : JsonPathComparisonResult.Child;
        
        if (jsonPath1.Path.Count < jsonPath2.Path.Count)
            return IsChildOfOrSameAs(jsonPath2, jsonPath1, out _) ? JsonPathComparisonResult.Parent : JsonPathComparisonResult.None;

        if (IsChildOfOrSameAs(jsonPath1, jsonPath2, out var numberOfMatchedPathElements))
            return JsonPathComparisonResult.Equal;

        if (jsonPath1.Path.Count == 1 || numberOfMatchedPathElements == jsonPath1.Path.Count - 1)
            return JsonPathComparisonResult.Sibling;
       
        return JsonPathComparisonResult.None;
    }

    private static bool IsChildOfOrSameAs(IJsonPath childJsonPath, IJsonPath parentJsonPath, out int numberOfMatchedPathElements)
    {
        numberOfMatchedPathElements = 0;

        if (parentJsonPath.Path.Count > childJsonPath.Path.Count)
            return false;

        for (var i = 0; i < parentJsonPath.Path.Count; ++i)
        {
            var parentPathElement = parentJsonPath.Path[i];
            var childPathElement = childJsonPath.Path[i];

            if (!parentPathElement.Equals(childPathElement))
                return false;

            ++numberOfMatchedPathElements;
        }
        
        return true;
    }
}