// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents the possible result types produced by a JSON function.
/// </summary>
public enum JsonFunctionResultType
{
    /// <summary>
    /// Numeric value. 
    /// </summary>
    Number,

    /// <summary>
    /// DateTime value. 
    /// </summary>
    DateTime,

    /// <summary>
    /// Boolean value
    /// </summary>
    Boolean,

    /// <summary>
    /// String value
    /// </summary>
    String,

    /// <summary>
    /// Json object
    /// </summary>
    JsonObject,

    /// <summary>
    /// Json array.
    /// </summary>
    JsonArray,

    /// <summary>
    /// Collection. This is normally result of JSON path function, such as "x.Where(y => y > 10)".
    /// </summary>
    Collection,

    /// <summary>
    /// Json null value
    /// </summary>
    JsonNull,

    /// <summary>
    /// Value is missing
    /// </summary>
    Undefined
}
