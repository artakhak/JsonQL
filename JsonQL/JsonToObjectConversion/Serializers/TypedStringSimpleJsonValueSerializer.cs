using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation.JsonValueTextGenerator;

namespace JsonQL.JsonToObjectConversion.Serializers;

public class TypedStringSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly IStringFormatter _stringFormatter;
    public Type SerializedType => typeof(string);

    public TypedStringSimpleJsonValueSerializer(IStringFormatter stringFormatter)
    {
        _stringFormatter = stringFormatter;
    }
    /// <inheritdoc />
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (_stringFormatter.TryFormat(value, out var formattedText))
        {
            serializedValue = formattedText;
            return true;
        }

        serializedValue = null;
        return false;
    }
}