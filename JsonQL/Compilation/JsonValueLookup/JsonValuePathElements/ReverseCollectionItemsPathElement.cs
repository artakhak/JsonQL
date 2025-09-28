// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;

/// <summary>
/// Represents a path element that selects items from a collection in reverse order.
/// </summary>
/// <remarks>
/// This class is used as a part of a JSON value lookup path, specifically for handling
/// operations that reverse the order of elements in a collection. It extends the
/// <c>JsonValueCollectionItemsSelectorPathElementAbstr</c> abstract class and provides
/// the specific implementation for reversing collection items.
/// </remarks>
/// <seealso cref="JsonValueCollectionItemsSelectorPathElementAbstr" />
public class ReverseCollectionItemsPathElement : JsonValueCollectionItemsSelectorPathElementAbstr
{
    /// <summary>
    /// Represents a path element that selects and reverses items within a JSON collection
    /// based on the provided function name and line information.
    /// </summary>
    public ReverseCollectionItemsPathElement(string selectorName, IJsonLineInfo? lineInfo) : base(selectorName, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parentParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        return new ParseResult<ICollectionJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(
            parentParsedValues.Reverse().ToList()));
    }
}
