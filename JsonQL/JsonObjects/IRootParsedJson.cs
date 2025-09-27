// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents the root object of a parsed JSON object model.
/// </summary>
/// <remarks>
/// This interface combines the capabilities of <see cref="IRootParsedValue"/> and <see cref="IParsedJson"/>,
/// serving as the root type in the parsed JSON hierarchy.
/// </remarks>
public interface IRootParsedJson : IRootParsedValue, IParsedJson
{

}