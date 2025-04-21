using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

public class TypedInt16SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    public Type SerializedType => typeof(Int16);

    /// <inheritdoc />
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is Int16 shortValue)
        {
            serializedValue = shortValue;
            return true;
        }

        if (value is string stringValue && Int16.TryParse(stringValue, out shortValue))
        {
            serializedValue = shortValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}