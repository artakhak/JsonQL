// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation.JsonValueTextGenerator;

namespace JsonQL.JsonToObjectConversion.Serializers;

/// <inheritdoc />
public class TypedStringSimpleJsonValueSerializer : ITypedSimpleJsonValueSerializer
{
    private readonly IStringFormatter _stringFormatter;
    
    /// <inheritdoc />
    public Type SerializedType => typeof(string);

    public TypedStringSimpleJsonValueSerializer(IStringFormatter stringFormatter)
    {
        _stringFormatter = stringFormatter;
    }
    
    /// <inheritdoc />
    public bool TrySerialize(object? value, [NotNullWhen(true)] out object? serializedValue)
    {
        if (value != null && _stringFormatter.TryFormat(value, out var formattedText))
        {
            serializedValue = formattedText;
            return true;
        }

        serializedValue = null;
        return false;
    }
}