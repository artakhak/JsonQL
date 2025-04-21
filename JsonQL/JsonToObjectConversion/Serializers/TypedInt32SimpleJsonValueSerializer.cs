using System;
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

public class TypedInt32SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    public Type SerializedType => typeof(Int32);

    /// <inheritdoc />
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is int intValue)
        {
            serializedValue = intValue;
            return true;
        }

        if (value is string stringValue && int.TryParse(stringValue, out intValue))
        {
            serializedValue = intValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}