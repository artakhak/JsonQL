using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

public class TypedBooleanSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    public Type SerializedType => typeof(bool);

    /// <inheritdoc />
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
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