// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents a result of a JSON value path lookup that retrieves a collection of items.<br/>
/// This interface is typically used to define operations or responses which involve multiple<br/>
/// JSON elements retrieved through a specified JSON path.<br/>
/// Examples are:<br/>
/// "parent.Object1.Array1.Flatten()", "parent.Object1.Array1.Flatten().Where(x => x.EmployeeId==11)",<br/>
/// "parent.Object1.Array1.Flatten().Reverse()", etc.
/// </summary>
public interface ICollectionJsonValuePathLookupResult: IJsonValuePathLookupResult
{
    /// <summary>
    /// Provides access to a collection of parsed values resulting from a JSON value path lookup.
    /// This property contains a read-only list of `IParsedValue` objects, representing the individual
    /// parsed components of the lookup result in a collection context.
    /// </summary>
    IReadOnlyList<IParsedValue> ParsedValues { get; }
}

/// <inheritdoc />
public class CollectionJsonValuePathLookupResult : ICollectionJsonValuePathLookupResult
{
    /// <summary>
    /// Represents the result of a lookup operation on a JSON value collection path.
    /// This class provides access to the parsed values resulting from a collection
    /// lookup operation. It serves as an implementation of the interface
    /// `ICollectionJsonValuePathLookupResult` and is used in various collection
    /// path element operations such as filtering, transformation, and selection
    /// of values.
    /// </summary>
    public CollectionJsonValuePathLookupResult(IReadOnlyList<IParsedValue> parsedValue)
    {
        ParsedValues = parsedValue;
    }

    /// <inheritdoc />
    public bool IsSingleItemLookup => false;

    /// <inheritdoc />
    public bool HasValue => ParsedValues.Count > 0;

    /// <inheritdoc />
    public IReadOnlyList<IParsedValue> ParsedValues { get; }
}