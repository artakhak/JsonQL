using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedGuidSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(Guid);

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is Guid guidValue)
        {
            serializedValue = guidValue;
            return true;
        }

        if (value is string stringValue && Guid.TryParse(stringValue, out guidValue))
        {
            serializedValue = guidValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}
