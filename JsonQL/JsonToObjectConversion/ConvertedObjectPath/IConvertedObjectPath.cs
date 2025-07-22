// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// Defines a contract for managing and constructing a converted object path,
/// consisting of a root element and a series of value selector elements.
public interface IConvertedObjectPath
{
    /// Represents the root element of a converted object path.
    /// This property provides access to the starting element
    /// of the object path in a JSON to object conversion context.
    IRootConvertedObjectPathElement RootConvertedObjectPathElement { get; }

    /// Represents the sequence of value selector elements within a converted object path.
    /// This property provides access to the ordered collection of path components
    /// that define the structure of the object path being converted.
    IReadOnlyList<IConvertedObjectPathValueSelectorElement> Path { get; }

    /// Adds a value selector element to the current object path.
    /// <param name="convertedObjectPathElement">The element to add to the object path. This element represents a part of the navigational path during JSON-to-object conversion.</param>
    void Push(IConvertedObjectPathValueSelectorElement convertedObjectPathElement);

    /// Removes and returns the last value selector element from the current object path.
    /// <returns>The element that was removed from the end of the object path.</returns>
    /// <exception cref="InvalidOperationException">Thrown when attempting to pop an element from an empty object path.</exception>
    IConvertedObjectPathValueSelectorElement Pop();

    /// Creates a copy of the current converted object path instance.
    /// The cloned object will include the root element and all value selector elements that are part of the current object path.
    /// This method ensures that the original converted object path remains unchanged during operations that involve the cloned instance.
    /// <returns>A new instance of the converted object path, containing the same root element and value selector elements as the original object path.</returns>
    IConvertedObjectPath Clone();
}

/// <inheritdoc />
public class ConvertedObjectPath : IConvertedObjectPath
{
    private readonly List<IConvertedObjectPathValueSelectorElement> _path = new();

    /// Represents a converted object path, managing a root element and a series
    /// of value selector elements used during JSON-to-object conversion.
    /// Provides functionality to add, remove, and clone elements within the path.
    /// Implements the IConvertedObjectPath interface.
    public ConvertedObjectPath(IRootConvertedObjectPathElement rootConvertedObjectPathElement)
    {
        RootConvertedObjectPathElement = rootConvertedObjectPathElement;
    }

    private ConvertedObjectPath(IRootConvertedObjectPathElement rootConvertedObjectPathElement, IReadOnlyList<IConvertedObjectPathValueSelectorElement> path)
    {
        RootConvertedObjectPathElement = rootConvertedObjectPathElement;
        _path = path.ToList();
    }

    /// <inheritdoc />
    public IRootConvertedObjectPathElement RootConvertedObjectPathElement { get; }

    /// <inheritdoc />
    public IReadOnlyList<IConvertedObjectPathValueSelectorElement> Path => _path;

    /// <inheritdoc />
    public void Push(IConvertedObjectPathValueSelectorElement convertedObjectPathElement)
    {
        _path.Add(convertedObjectPathElement);
    }

    /// <inheritdoc />
    public IConvertedObjectPathValueSelectorElement Pop()
    {
        if (_path.Count == 0)
            throw new InvalidOperationException($"The stack [{nameof(Path)}] is empty.");

        var convertedObjectPathElement = _path[^1];

        _path.RemoveAt(_path.Count - 1);
        return convertedObjectPathElement;
    }

    /// <inheritdoc />
    public IConvertedObjectPath Clone()
    {
        var path = new List<IConvertedObjectPathValueSelectorElement>();

        foreach (var pathElement in this._path)
            path.Add(pathElement.Clone());

        return new ConvertedObjectPath(RootConvertedObjectPathElement, path);
    }
}
