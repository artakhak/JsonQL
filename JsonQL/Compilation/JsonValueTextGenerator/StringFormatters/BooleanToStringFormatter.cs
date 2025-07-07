// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;

/// <inheritdoc />
public class BooleanToStringFormatter : IStringFormatter
{
    /// <inheritdoc />
    public bool TryFormat(object value, [NotNullWhen(true)] out string? formattedValue)
    {
        formattedValue = null;

        if (value is not bool booleanValue)
            return false;

        formattedValue = booleanValue ? "true" : "false";
        return true;
    }
}