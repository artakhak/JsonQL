// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedFloatSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(float);

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is float floatValue)
        {
            serializedValue = floatValue;
            return true;
        }

        if (value is string stringValue && float.TryParse(stringValue, out floatValue))
        {
            serializedValue = floatValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}
