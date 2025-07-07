// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// <summary>
/// Represents an element in a converted object path used for value selection
/// during JSON-to-object conversion. This interface is a specialized extension
/// of <see cref="IConvertedObjectPathElement"/> that introduces functionality
/// for working with value selector elements.
/// </summary>
public interface IConvertedObjectPathValueSelectorElement : IConvertedObjectPathElement
{
    /// <summary>
    /// Creates a new instance of the object that is a copy of the current instance.
    /// </summary>
    /// <returns>
    /// A new instance of <see cref="IConvertedObjectPathValueSelectorElement"/> that is a copy of the current instance.
    /// </returns>
    IConvertedObjectPathValueSelectorElement Clone();
}