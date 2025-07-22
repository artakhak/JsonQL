// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonObjects.JsonPath;

/// <summary>
/// Represents a JSON property name path element within a JSON path.
/// </summary>
public interface IJsonPropertyNamePathElement : IJsonPathElement
{
    /// <summary>
    /// Gets the name of the JSON property represented by this path element.
    /// </summary>
    string Name { get; }
}

/// <inheritdoc />
public class JsonPropertyNamePathElement : IJsonPropertyNamePathElement
{
    /// <summary>
    /// Represents an element in a JSON path specifically defined by a property name.
    /// This class is used to define named property segments in a JSON path structure.
    /// </summary>
    public JsonPropertyNamePathElement(string name)
    {
        Name = name;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc />
    public bool Equals(IJsonPathElement jsonPathElement)
    {
        if (jsonPathElement is not JsonPropertyNamePathElement jsonPropertyNamePathElement)
            return false;

        return string.Equals(this.Name, jsonPropertyNamePathElement.Name, StringComparison.Ordinal);
    }
}
