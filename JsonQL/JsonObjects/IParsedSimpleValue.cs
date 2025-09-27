// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a parsed JSON value that is a simple, non-composite type.
/// This interface is used to model simple JSON data types such as strings, numbers,
/// and boolean values. It includes properties that provide additional
/// information about the value, such as whether it is a string.
/// </summary>
public interface IParsedSimpleValue : IParsedValue
{
    /// <summary>
    /// Represents the value of a simple, non-composite JSON type.
    /// This property holds the actual content of the parsed JSON, which can be a string, number, or boolean.
    /// If the value is a string, it will be quoted when serialized. If null, it represents a "null" JSON value.
    /// </summary>
    string? Value { get; set; }

    /// <summary>
    /// If the value is true, the serialized value will be enclosed onm apostrophes. Example: "BusName": "DefaultMessageHandler".
    /// Otherwise, the value will not be enclosed in  apostrophes. Example: "RebusNumberOfWorkers": 10.
    /// </summary>
    public bool IsString { get; }
}