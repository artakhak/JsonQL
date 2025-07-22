// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedDateTimeSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    /// <inheritdoc />
    public Type SerializedType => typeof(DateTime);

    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value is DateTime dateTime)
        {
            serializedValue = dateTime;
            return true;
        }
        
        if (value is string stringValue && DateTime.TryParse(stringValue, out dateTime))
        {
            serializedValue = dateTime;
            return true;
        }

        serializedValue = null;
        return false;
    }
}
