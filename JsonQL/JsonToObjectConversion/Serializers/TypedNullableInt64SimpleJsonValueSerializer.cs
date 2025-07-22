using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableInt64SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly TypedInt64SimpleJsonValueSerializer _typedInt64SimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(Int64?);

    public TypedNullableInt64SimpleJsonValueSerializer(TypedInt64SimpleJsonValueSerializer typedInt64SimpleJsonValueSerializer)
    {
        _typedInt64SimpleJsonValueSerializer = typedInt64SimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedInt64SimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}
