using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableDateTimeSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly TypedDateTimeSimpleJsonValueSerializer _typedDateTimeSimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(DateTime?);

    public TypedNullableDateTimeSimpleJsonValueSerializer(TypedDateTimeSimpleJsonValueSerializer typedDateTimeSimpleJsonValueSerializer)
    {
        _typedDateTimeSimpleJsonValueSerializer = typedDateTimeSimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedDateTimeSimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}