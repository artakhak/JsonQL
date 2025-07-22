// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedInt32SimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(Int32);

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is int intValue)
        {
            serializedValue = intValue;
            return true;
        }

        if (value is string stringValue && int.TryParse(stringValue, out intValue))
        {
            serializedValue = intValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}
