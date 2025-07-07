// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Reflection;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents a data structure containing information about a property of a model class during its creation process.
/// </summary>
public interface IModelClassCreationPropertyData
{
    /// <summary>
    /// Gets metadata information about a property of a class, such as its name, type, and attributes.
    /// </summary>
    public PropertyInfo PropertyInfo { get; }

    /// <summary>
    /// Gets or sets the value assigned to a property during the creation of a model class instance.
    /// </summary>
    public object? PropertyValue { get; }
}

// <inheritdoc />
public class ModelClassCreationPropertyData : IModelClassCreationPropertyData
{
    /// Represents a property and its associated value for a model class during object creation from JSON.
    /// Used to store the mapping between a property's metadata and its deserialized value.
    public ModelClassCreationPropertyData(PropertyInfo propertyInfo, object? propertyValue)
    {
        PropertyInfo = propertyInfo;
        PropertyValue = propertyValue;
    }

    /// <inheritdoc />
    public PropertyInfo PropertyInfo { get; }

    // <inheritdoc />
    public object? PropertyValue { get; }
}