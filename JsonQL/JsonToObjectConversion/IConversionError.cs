// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects.JsonPath;
using JsonQL.JsonToObjectConversion.ConvertedObjectPath;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Conversion error details.
/// </summary>
public interface IConversionError
{
    /// <summary>
    /// Error type.
    /// </summary>
    ConversionErrorType ErrorType { get; }

    /// <summary>
    /// If the value is not null, parsed JSON value path associated with error.
    /// </summary>
    IJsonPath? JsonPath { get; }

     /// <summary>
     /// If the value is not null, a JSON path that points out to the original JSON value.
     /// </summary>
     IJsonPath? PathInReferencedJson { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    string Error { get; }

    /// <summary>
    /// If the value is not null, path describing the object path.<br/>
    /// Example: ["Root, "[0]", "Address", "Street"] for an expression like "Employees[0].Address.Street".
    /// </summary>
    IConvertedObjectPath? ConvertedObjectPath { get; }
}