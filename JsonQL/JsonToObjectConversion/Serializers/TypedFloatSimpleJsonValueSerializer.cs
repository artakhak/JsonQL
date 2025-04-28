using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedFloatSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(float);

    /// <inheritdoc />
    public bool TrySerialize(object value, [NotNullWhen(true)] out object? serializedValue)
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