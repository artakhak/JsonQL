// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a parsed root value for a JSON array. This interface combines the functionality
/// of <see cref="IRootParsedValue"/> and <see cref="IParsedArrayValue"/>, allowing it to
/// function both as a root parsed value and as an array of parsed values.
/// </summary>
/// <remarks>
/// Implementations of this interface are expected to support operations specific to root level
/// JSON arrays, including value access by index, addition, and removal, as well as functionality
/// specific to root parsed values such as tracking and retrieving parsed values by their unique IDs.
/// </remarks>
public interface IRootParsedArrayValue : IRootParsedValue, IParsedArrayValue
{

}