namespace JsonQL.JsonObjects.JsonPath;

public static class JsonPathHelpers
{
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