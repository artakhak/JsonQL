using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Defines a contract for serializing a JSON value to a specified .NET type.
/// </summary>
public interface ISimpleJsonValueSerializer
{
    /// <summary>
    /// Attempts to serialize a given value to the specified type using a registered serializer.
    /// </summary>
    /// <param name="typeToDeserializeTo">The target type to which the value should be serialized.</param>
    /// <param name="value">The value to be serialized.</param>
    /// <param name="serializedValue">The serialized output value if the operation is successful; otherwise, null.</param>
    /// <returns>True if the serialization is successful; otherwise, false.</returns>
    bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue);
}

/// <inheritdoc />
public class AggregateSimpleJsonValueSerializer : ISimpleJsonValueSerializer
{
    private readonly Dictionary<Type, ITypedSimpleJsonValueSerializer> _typeToSerializerMap = new();

    /// <summary>
    /// Aggregates multiple typed simple JSON value serializers into a single composite serializer.
    /// </summary>
    /// <remarks>
    /// This class allows the serialization and deserialization of various simple JSON values by combining
    /// multiple implementations of <see cref="ITypedSimpleJsonValueSerializer"/>. Each serializer is mapped
    /// to a specific .NET type, facilitating a unified interface for handling diverse types.
    /// </remarks>
    /// <param name="typedSimpleJsonValueSerializers">
    /// A collection of typed simple JSON value serializers to be aggregated.
    /// </param>
    public AggregateSimpleJsonValueSerializer(IReadOnlyList<ITypedSimpleJsonValueSerializer> typedSimpleJsonValueSerializers)
    {
        foreach (var typedSimpleJsonValueSerializer in typedSimpleJsonValueSerializers)
            _typeToSerializerMap[typedSimpleJsonValueSerializer.SerializedType] = typedSimpleJsonValueSerializer;
    }
    
    /// <inheritdoc />
    public bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (_typeToSerializerMap.TryGetValue(typeToDeserializeTo, out var typedSimpleJsonValueSerializer) &&
            typedSimpleJsonValueSerializer.TrySerialize(value, out serializedValue))
        {
            return true;
        }

        serializedValue = null;
        return false;
    }
}