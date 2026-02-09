using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedNullableGuidSimpleJsonValueSerializer: ITypedSimpleJsonValueSerializer
{
    private readonly TypedGuidSimpleJsonValueSerializer _typedGuidSimpleJsonValueSerializer;

    /// <inheritdoc />
    public Type SerializedType => typeof(double?);

    public TypedNullableGuidSimpleJsonValueSerializer(TypedGuidSimpleJsonValueSerializer typedGuidSimpleJsonValueSerializer)
    {
        _typedGuidSimpleJsonValueSerializer = typedGuidSimpleJsonValueSerializer;
    }

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value == null)
        {
            serializedValue = true;
            return true;
        }

        return _typedGuidSimpleJsonValueSerializer.TrySerialize(value, out serializedValue);
    }
}
