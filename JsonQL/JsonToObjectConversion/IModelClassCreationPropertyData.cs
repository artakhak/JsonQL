using System.Reflection;

namespace JsonQL.JsonToObjectConversion;

public interface IModelClassCreationPropertyData
{
    public PropertyInfo PropertyInfo { get; }
    public object? PropertyValue { get; }
}

// <inheritdoc />
public class ModelClassCreationPropertyData : IModelClassCreationPropertyData
{
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