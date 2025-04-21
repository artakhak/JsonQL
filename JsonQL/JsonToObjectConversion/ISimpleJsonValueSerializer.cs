using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

public interface ISimpleJsonValueSerializer
{
    bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue);
}

public class AggregateSimpleJsonValueSerializer : ISimpleJsonValueSerializer
{
    private readonly Dictionary<Type, ITypedSimpleJsonValueSerializer> _typeToSerializerMap = new();

    public AggregateSimpleJsonValueSerializer(IReadOnlyList<ITypedSimpleJsonValueSerializer> typedSimpleJsonValueSerializers)
    {
        foreach (var typedSimpleJsonValueSerializer in typedSimpleJsonValueSerializers)
            _typeToSerializerMap[typedSimpleJsonValueSerializer.SerializedType] = typedSimpleJsonValueSerializer;
    }
    
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (_typeToSerializerMap.TryGetValue(typeToDeserializeTo, out var typedSimpleJsonValueSerializer) &&
            typedSimpleJsonValueSerializer.TrySerialize(typeToDeserializeTo, value, out serializedValue))
        {
            return true;
        }

        serializedValue = null;
        return false;
    }
}