// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedInt16SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(Int16);

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is Int16 shortValue)
        {
            serializedValue = shortValue;
            return true;
        }

        if (value is string stringValue && Int16.TryParse(stringValue, out shortValue))
        {
            serializedValue = shortValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}