using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedBooleanSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(bool);

    /// <inheritdoc />
    public bool TrySerialize(object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is bool booleanValue)
        {
            serializedValue = booleanValue;
            return true;
        }
        
        if (value is string stringValue && bool.TryParse(stringValue, out booleanValue))
        {
            serializedValue = booleanValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}