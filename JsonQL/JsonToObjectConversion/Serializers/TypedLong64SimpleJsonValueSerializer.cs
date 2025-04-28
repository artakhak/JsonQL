using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedInt64SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(Int64);

    /// <inheritdoc />
    public bool TrySerialize(object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is long longValue)
        {
            serializedValue = longValue;
            return true;
        }

        if (value is string stringValue && long.TryParse(stringValue, out longValue))
        {
            serializedValue = longValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}