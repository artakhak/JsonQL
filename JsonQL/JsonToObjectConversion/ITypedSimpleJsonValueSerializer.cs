using System;
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

public interface ITypedSimpleJsonValueSerializer
{
    Type SerializedType { get; }

    bool TrySerialize(Type typeToDeserializeTo, object value, [NotNullWhen(true)] out object? serializedValue);
}