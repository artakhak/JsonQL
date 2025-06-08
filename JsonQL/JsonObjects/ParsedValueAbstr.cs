using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents an abstract base class for parsed values in a JSON structure.
/// Implements the <see cref="IParsedValue"/> interface and provides shared functionality
/// for handling JSON path resolution, parent-child relationships, and JSON metadata.
/// </summary>
public abstract class ParsedValueAbstr: IParsedValue
{
    private IJsonPath? _jsonPath;

    /// <summary>
    /// Constructor. 
    /// </summary>
    /// <param name="parentJsonValue">
    /// If the value of <see cref="ParentJsonValue"/> is not null, then the value of <see cref="JsonKeyValue"/> should be null, and vice versa.
    /// </param>
    /// <param name="jsonKeyValue">
    /// If the value of <see cref="ParentJsonValue"/> is not null, then the value of <see cref="JsonKeyValue"/> should be null, and vice versa.
    /// </param>
    /// <param name="pathInReferencedJson">
    /// If the value is not null, a json path that points out to original json value.<br/>
    /// For example, consider the following two json files "Employees.json" that has a JSON array value at JSON key "Employees" (e.g., {"Employees": [...]}<br/>
    /// Also, lets assume we have another JSON "EmployeeExpressions.json" shown below that has an expression that references the second employee in "Employees.json".<br/>
    /// {"SecondEmployee": "$value(Employees[1])"}.<br/>
    /// In this example the value returned by method call <see cref="IParsedValue.GetPath"/> for JSON value "Employees[1]" will be "Root.SecondEmployee" since it is a json value in<br/>
    /// "EmployeeExpressions.json" mapped to key "Root.SecondEmployee". However, the value of <see cref="PathInReferencedJson"/> will be<br/>
    /// "Root.Employees[1]" and <see cref="PathInReferencedJson"/>.<see cref="IJsonPath.JsonTextIdentifier"/> will be "Employees.json".
    /// </param>
    protected ParsedValueAbstr(IParsedValue? parentJsonValue, IJsonKeyValue? jsonKeyValue, IJsonPath? pathInReferencedJson)
    {
        Id = Guid.NewGuid();
        ParentJsonValue = parentJsonValue;
        JsonKeyValue = jsonKeyValue;
        PathInReferencedJson = pathInReferencedJson;
    }

    /// <inheritdoc />
    public IJsonPath GetPath()
    {
        return _jsonPath ??= CalculateJsonPath();
    }

    /// <inheritdoc />
    public IJsonPath? PathInReferencedJson { get; }

    private IJsonPath CalculateJsonPath()
    {
        var jsonTextIdentifier = this.RootParsedValue.JsonTextIdentifier;

        if (ParentJsonValue == null)
            return new JsonPath.JsonPath(jsonTextIdentifier, CollectionExpressionHelpers.Create(new JsonPropertyNamePathElement(JsonObjectConstants.RootValuePathName)));

        var parentPath = this.ParentJsonValue.GetPath();

        var jsonPathElements = new List<IJsonPathElement>(parentPath.Path.Count + 1);
        jsonPathElements.AddRange(parentPath.Path);

        if (JsonKeyValue is not null)
        {
            jsonPathElements.Add(new JsonPropertyNamePathElement(JsonKeyValue.Key));
            return new JsonPath.JsonPath(jsonTextIdentifier, jsonPathElements);
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
                ThreadStaticLoggingContext.Context.ErrorFormat("Index of value with Id={0} not found in parent array with Id={1}.",
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
            return new JsonPath.JsonPath(jsonTextIdentifier, jsonPathElements);
        }

        return new JsonPath.JsonPath(jsonTextIdentifier, CollectionExpressionHelpers.Create(new JsonPropertyNamePathElement(string.Empty)));
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