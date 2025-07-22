// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// Defines a factory for creating instances of IPropertyNameConvertedObjectPathElement.
/// Provides a method to generate elements associated with a specific property name
/// and its corresponding object type within a converted object path context.
public interface IPropertyNameConvertedObjectPathElementFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IPropertyNameConvertedObjectPathElement"/>
    /// associated with the specified property name and its corresponding object type.
    /// </summary>
    /// <param name="propertyName">The name of the property for which the element is being created.</param>
    /// <param name="objectType">The type of the object associated with the property.</param>
    /// <returns>An instance of <see cref="IPropertyNameConvertedObjectPathElement"/> representing the specified property name and object type.</returns>
    IPropertyNameConvertedObjectPathElement Create(string propertyName, Type objectType);
}

/// <inheritdoc />
public class PropertyNameConvertedObjectPathElementFactory : IPropertyNameConvertedObjectPathElementFactory
{
    /// <inheritdoc />
    public IPropertyNameConvertedObjectPathElement Create(string propertyName, Type objectType)
    {
        return new PropertyNameConvertedObjectPathElement(propertyName, objectType);
    }
}
