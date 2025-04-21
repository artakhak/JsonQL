using System;
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

public class TypedDoubleSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    public Type SerializedType => typeof(double);

    /// <inheritdoc />
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is double doubleValue)
        {
            serializedValue = doubleValue;
            return true;
        }

        if (value is string stringValue && double.TryParse(stringValue, out doubleValue))
        {
            serializedValue = doubleValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}

public class TypedFloatSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    public Type SerializedType => typeof(float);

    /// <inheritdoc />
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is float floatValue)
        {
            serializedValue = floatValue;
            return true;
        }

        if (value is string stringValue && float.TryParse(stringValue, out floatValue))
        {
            serializedValue = floatValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}