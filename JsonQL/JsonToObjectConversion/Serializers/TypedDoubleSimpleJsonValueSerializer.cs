// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedDoubleSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(double);

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is double doubleValue)
        {
            serializedValue = doubleValue;
            return true;
        }

        if (value is string stringValue && double.TryParse(stringValue, out doubleValue))
        {
            serializedValue = doubleValue;
            return true;
        }

        serializedValue = null;
        return false;
    }
}