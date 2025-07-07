// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonToObjectConversion.ConvertedObjectPath;

/// <summary>
/// Represents an element within a converted object path that is specifically
/// associated with property names. This interface is based on
/// <see cref="IConvertedObjectPathValueSelectorElement"/> and is used during
/// JSON-to-object conversion to denote properties within object paths.
/// </summary>
public interface IPropertyNameConvertedObjectPathElement : IConvertedObjectPathValueSelectorElement
{

}

/// <summary>
/// Represents a concrete element within a converted object path, specifically tied to property names.
/// This class extends the functionality of <see cref="ConvertedObjectPathValueSelectorElementAbstr"/> and implements
/// <see cref="IPropertyNameConvertedObjectPathElement"/> to provide property-specific behavior during
/// JSON-to-object conversion processes. Each instance is associated with a property name and its target object type.
/// </summary>
public class PropertyNameConvertedObjectPathElement : ConvertedObjectPathValueSelectorElementAbstr, IPropertyNameConvertedObjectPathElement
{
    private readonly string _propertyName;

    /// <summary>
    /// Represents an element within a converted object path, associated with a specific property name
    /// and capable of selecting a value during JSON-to-object conversion.
    /// </summary>
    public PropertyNameConvertedObjectPathElement(string propertyName, Type objectType) : base(propertyName, objectType)
    {
        _propertyName = propertyName;
    }

    /// <inheritdoc />
    public override IConvertedObjectPathValueSelectorElement Clone()
    {
        return new PropertyNameConvertedObjectPathElement(_propertyName, this.ObjectType);
    }
}