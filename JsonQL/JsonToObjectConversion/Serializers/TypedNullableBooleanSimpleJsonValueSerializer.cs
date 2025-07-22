using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableBooleanSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly TypedBooleanSimpleJsonValueSerializer _typedBooleanSimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(bool?);

    public TypedNullableBooleanSimpleJsonValueSerializer(TypedBooleanSimpleJsonValueSerializer typedDoubleSimpleJsonValueSerializer)
    {
        _typedBooleanSimpleJsonValueSerializer = typedDoubleSimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedBooleanSimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}
