using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableInt16SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly TypedInt16SimpleJsonValueSerializer _typedInt16SimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(Int16?);

    public TypedNullableInt16SimpleJsonValueSerializer(TypedInt16SimpleJsonValueSerializer typedInt16SimpleJsonValueSerializer)
    {
        _typedInt16SimpleJsonValueSerializer = typedInt16SimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedInt16SimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}