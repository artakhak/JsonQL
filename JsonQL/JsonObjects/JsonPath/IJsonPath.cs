using System.Text;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;

namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Represents a JSON path used to navigate data within a JSON structure.
/// </summary>
public interface IJsonPath
{
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
    public JsonPath(IReadOnlyList<IJsonPathElement> path)
    {
        Path = path;

        var pathToString = new StringBuilder();

        // IJsonPathElement have elements like a, b, [1, 2], which should be converted
        // to "a.b[1, 2]"

        var pathElementIndex = 0;

        while (pathElementIndex < path.Count)
        {
            var pathElement = path[pathElementIndex];

            if (pathElement is IJsonPropertyNamePathElement propertyNamePathElement &&
                pathElementIndex < path.Count - 1 && path[pathElementIndex + 1] is IJsonArrayIndexesPathElement jsonArrayIndexesPathElement)
            {
                pathToString.Append(propertyNamePathElement);
                pathToString.Append(jsonArrayIndexesPathElement);

                // Skip the array indexes since we already added this.
                pathElementIndex += 2;
                continue;
            }

            if (pathElementIndex > 0)
            {
                pathToString.Append(JsonOperatorNames.JsonPathSeparator);

            }
            pathToString.Append(pathElement);

            ++pathElementIndex;
        }

        _pathToString = pathToString.ToString();
    }

    /// <inheritdoc />
    public IReadOnlyList<IJsonPathElement> Path { get; }

    /// <inheritdoc />
    public override string ToString() => _pathToString;
}