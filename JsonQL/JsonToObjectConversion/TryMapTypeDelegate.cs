// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Maps type used to convert <see cref="IParsedJson"/> to an object to a different type, such as interface implementation or a subclass.
/// </summary>
/// <param name="defaultTypeToConvertParsedJsonTo">
/// Type to which <paramref name="convertedParsedJson"/> is converted to.
/// </param>
/// <param name="convertedParsedJson">Parsed json object being converted to ann instance of <paramref name="defaultTypeToConvertParsedJsonTo"/>.</param>
/// <returns>
/// Either returns non-null type to use when converting <paramref name="convertedParsedJson"/>
/// to an instance of <paramref name="defaultTypeToConvertParsedJsonTo"/>, or returns null, if no custom mapping exists. IN case null is returned,
/// <see cref="IModelClassMapper.TryMap"/> will be used to map the type.
/// </returns>
public delegate Type? TryMapTypeDelegate(Type defaultTypeToConvertParsedJsonTo, IParsedJson convertedParsedJson);
