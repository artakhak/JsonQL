// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.JsonObjects;

/// <summary>
/// Defines a mechanism to create a copy of an <see cref="IParsedValue"/> instance while associating it with a new parent.
/// </summary>
public interface IParsedValueCopy
{
    /// <summary>
    /// Creates a copy of the specified <see cref="IParsedValue"/> while associating it with a new parent and optionally a new <see cref="IJsonKeyValue"/>.
    /// </summary>
    /// <param name="parsedValue">The original <see cref="IParsedValue"/> to be copied.</param>
    /// <param name="parentParsedValue">The new parent <see cref="IParsedValue"/> that the copy will be associated with.</param>
    /// <param name="jsonKeyValue">An optional <see cref="IJsonKeyValue"/> to associate with the copied value.</param>
    /// <returns>Returns a new instance of <see cref="IParsedValue"/> that is a copy of the original and linked to the provided new parent and optional key-value pair.</returns>
    IParsedValue CopyWithNewParent(IParsedValue parsedValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue);
}