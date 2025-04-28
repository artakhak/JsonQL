using JsonQL.JsonObjects.JsonPath;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents an abstract base class for parsed values in a JSON structure.
/// Implements the <see cref="IParsedValue"/> interface and provides shared functionality
/// for handling JSON path resolution, parent-child relationships, and JSON metadata.
/// </summary>
public abstract class ParsedValueAbstr: IParsedValue
{
    protected ParsedValueAbstr(IParsedValue? parentJsonValue, IJsonKeyValue? jsonKeyValue)
    {
        Id = Guid.NewGuid();
        ParentJsonValue = parentJsonValue;
        JsonKeyValue = jsonKeyValue;
    }

    /// <inheritdoc />
    public IJsonPath GetPath()
    {
        if (ParentJsonValue == null)
            return new JsonPath.JsonPath(CollectionExpressionHelpers.Create(new JsonPropertyNamePathElement(JsonObjectConstants.RootValuePathName)));

        var parentPath = this.ParentJsonValue.GetPath();

        var jsonPathElements = new List<IJsonPathElement>(parentPath.Path.Count + 1);
        jsonPathElements.AddRange(parentPath.Path);

        if (JsonKeyValue is not null)
        {
            jsonPathElements.Add(new JsonPropertyNamePathElement(JsonKeyValue.Key));
            return new JsonPath.JsonPath(jsonPathElements);
        }

        if (ParentJsonValue is IParsedArrayValue parentParsedArrayValue)
        {
            List<int> parsedArrayValueIndexInParentArray = new List<int>();

            if (jsonPathElements.Count > 0)
            {
                var lastPathElement = jsonPathElements[^1];

                if (lastPathElement is IJsonArrayIndexesPathElement jsonArrayIndexesPathElement)
                    parsedArrayValueIndexInParentArray.AddRange(jsonArrayIndexesPathElement.Indexes);
            }

            if (!parentParsedArrayValue.TryGetValueIndex(this.Id, out var indexInArray))
            {
                ThreadStaticLogging.Log.ErrorFormat("Index of value with Id={0} not found in parent array with Id={1}.",
                    this.Id, parentParsedArrayValue.Id);
                indexInArray = -1;
            }

            parsedArrayValueIndexInParentArray.Add(indexInArray.Value);

            while (jsonPathElements.Count > 0 && jsonPathElements[^1] is IJsonArrayIndexesPathElement)
            {
                // We use array indexes in the format a[1, 2, 3] and not a[1][2][3]
                // so lets remove parent indexes from the path
                jsonPathElements.RemoveAt(jsonPathElements.Count - 1);
            }

            jsonPathElements.Add(new JsonArrayIndexesPathElement(parsedArrayValueIndexInParentArray));
            return new JsonPath.JsonPath(jsonPathElements);
        }

        return new JsonPath.JsonPath(CollectionExpressionHelpers.Create(new JsonPropertyNamePathElement(string.Empty)));
    }

    /// <inheritdoc />
    public Guid Id { get; }

    /// <inheritdoc />
    public abstract IRootParsedValue RootParsedValue { get; }

    /// <inheritdoc />
    public IParsedValue? ParentJsonValue { get; }

    /// <inheritdoc />
    public IJsonKeyValue? JsonKeyValue { get; }

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; set; }
}