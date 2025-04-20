using JsonQL.JsonExpression;
using System.Text;

namespace JsonQL.JsonObjects.JsonPath;

public interface IJsonPath
{
    IReadOnlyList<IJsonPathElement> Path { get; }
}

/// <inheritdoc />
public class JsonPath : IJsonPath
{
    private readonly string _pathToString;

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