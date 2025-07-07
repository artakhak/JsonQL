// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// A lookup by a path that selects multiple items. Examples are items selected by paths like this:<br/>
/// "parent.Object1.Array1", "Object1.Array1", "Object1.Array1[1, 3]", "Object1.Array1.Where(x => x.EmployeeId==11).At(1)"<br/>
/// "Object1.Array1.Where(x => x.EmployeeId==11).First(1)", etc.
/// </summary>
public interface ISingleItemJsonValuePathLookupResult: IJsonValuePathLookupResult
{
    /// <summary>
    /// Gets the parsed value associated with the JSON value path lookup result.
    /// </summary>
    IParsedValue? ParsedValue { get; }

    /// <summary>
    /// Gets a value indicating whether the specified JSON path is valid or not.
    /// </summary>
    bool IsValidPath { get; }
}

/// <inheritdoc />
public class SingleItemJsonValuePathLookupResult : ISingleItemJsonValuePathLookupResult
{
    private SingleItemJsonValuePathLookupResult(bool isValidPath,IParsedValue? parsedValue)
    {
        IsValidPath = isValidPath;
        ParsedValue = parsedValue;
    }

    /// <summary>
    /// Creates a new instance of <see cref="SingleItemJsonValuePathLookupResult"/> for a valid path
    /// based on the provided parsed value.
    /// </summary>
    /// <param name="parsedValue">The parsed value representing a valid JSON path element. Can be null.</param>
    /// <returns>A new instance of <see cref="SingleItemJsonValuePathLookupResult"/> representing a valid path.</returns>
    public static SingleItemJsonValuePathLookupResult CreateForValidPath(IParsedValue? parsedValue)
    {
        return new SingleItemJsonValuePathLookupResult(true, parsedValue);
    }

    /// <summary>
    /// Creates a new instance of <see cref="SingleItemJsonValuePathLookupResult"/> representing an invalid path.
    /// </summary>
    /// <returns>A new instance of <see cref="SingleItemJsonValuePathLookupResult"/> indicating an invalid path.</returns>
    public static SingleItemJsonValuePathLookupResult CreateForInvalidPath()
    {
        return new SingleItemJsonValuePathLookupResult(false, null);
    }

    /// <inheritdoc />
    public bool IsSingleItemLookup => true;

    /// <inheritdoc />
    public bool HasValue => this.ParsedValue != null;

    /// <inheritdoc />
    public IParsedValue? ParsedValue { get; }

    /// <inheritdoc />
    public bool IsValidPath { get; }
}