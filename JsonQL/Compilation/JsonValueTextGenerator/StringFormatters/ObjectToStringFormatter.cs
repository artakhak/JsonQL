// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;

/// <inheritdoc />
public class ObjectToStringFormatter: IStringFormatter
{
    /// <inheritdoc />
    public bool TryFormat(object value, [NotNullWhen(true)] out string? formattedValue)
    {
        formattedValue = value.ToString();
        return formattedValue != null;
    }
}
