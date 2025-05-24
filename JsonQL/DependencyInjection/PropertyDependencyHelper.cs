namespace JsonQL.DependencyInjection;

/// <summary>
/// Provides utility methods for handling property dependencies in objects.
/// </summary>
public static class PropertyDependencyHelper
{
    /// <summary>
    /// Sets a property on the given object using reflection where the property type matches the type of the provided value.
    /// </summary>
    /// <typeparam name="TObjectWIthSetter">The type of the object on which the property is to be set. The object must be a reference type and contain a writable property with a type matching <typeparamref name="TPropertyType"/>.</typeparam>
    /// <typeparam name="TPropertyType">The type of the property value to set. This must be a reference type.</typeparam>
    /// <param name="objectWithSetter">The object containing the property to set.</param>
    /// <param name="propertyValue">The value to set to the matching writable property.</param>
    /// <returns>The modified object after setting the specified property value. If no matching property is found, the object is returned unmodified.</returns>
    public static TObjectWIthSetter SetJsonFunctionFromExpressionParser<TObjectWIthSetter, TPropertyType>(TObjectWIthSetter objectWithSetter, TPropertyType propertyValue) 
        where TObjectWIthSetter: class
        where TPropertyType: class
    {
        var propertyInfo = objectWithSetter.GetType().GetProperties().FirstOrDefault(x => x.CanWrite &&
                                                                                          x.PropertyType == propertyValue.GetType());

        if (propertyInfo != null)
            propertyInfo.SetValue(objectWithSetter, propertyValue);
        return objectWithSetter;
    }
}