// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Reflection;

namespace JsonQL.DependencyInjection;

/// <summary>
/// Provides utility methods for handling property dependencies in objects.
/// </summary>
public static class PropertyDependencyHelper
{
    /// <summary>
    /// Sets a properties on the given object using reflection where the property type is assignable from the type of the provided value.
    /// </summary>
    /// <typeparam name="TObjectWithSetter">The type of the object on which the property is to be set. The object must be a reference type and contain a writable property with a type matching <typeparamref name="TPropertyType"/>.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property value to set. This must be a reference type.</typeparam>
    /// <param name="objectWithSetter">The object containing the property to set.</param>
    /// <param name="propertyValue">The value to set to the matching writable property.</param>
    /// <returns>The modified object after setting the specified property value. If no matching property is found, the object is returned unmodified.</returns>
    public static TObjectWithSetter SetJsonFunctionFromExpressionParser<TObjectWithSetter, TPropertyType>(TObjectWithSetter objectWithSetter, TPropertyType propertyValue) 
        where TObjectWithSetter: class
        where TPropertyType: class
    {
        var propertyValueType = propertyValue.GetType();

        var propertiesToSet = objectWithSetter.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.CanWrite && x.PropertyType.IsAssignableFrom(propertyValueType));

        foreach (var propertyInfo in propertiesToSet)
            propertyInfo.SetValue(objectWithSetter, propertyValue);
       
        return objectWithSetter;
    }
}