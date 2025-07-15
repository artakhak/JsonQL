using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableFloatSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly TypedFloatSimpleJsonValueSerializer _typedFloatSimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(float?);

    public TypedNullableFloatSimpleJsonValueSerializer(TypedFloatSimpleJsonValueSerializer typedFloatSimpleJsonValueSerializer)
    {
        _typedFloatSimpleJsonValueSerializer = typedFloatSimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedFloatSimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}