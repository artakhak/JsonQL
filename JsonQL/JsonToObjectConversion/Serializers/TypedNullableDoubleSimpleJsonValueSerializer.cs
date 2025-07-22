using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableDoubleSimpleJsonValueSerializer: ITypedSimpleJsonValueSerializer
{
    private readonly TypedDoubleSimpleJsonValueSerializer _typedDoubleSimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(double?);

    public TypedNullableDoubleSimpleJsonValueSerializer(TypedDoubleSimpleJsonValueSerializer typedDoubleSimpleJsonValueSerializer)
    {
        _typedDoubleSimpleJsonValueSerializer = typedDoubleSimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedDoubleSimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}
