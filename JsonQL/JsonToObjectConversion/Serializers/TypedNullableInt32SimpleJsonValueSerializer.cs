using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableInt32SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly TypedInt32SimpleJsonValueSerializer _typedInt32SimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(Int32?);

    public TypedNullableInt32SimpleJsonValueSerializer(TypedInt32SimpleJsonValueSerializer typedInt32SimpleJsonValueSerializer)
    {
        _typedInt32SimpleJsonValueSerializer = typedInt32SimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedInt32SimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}