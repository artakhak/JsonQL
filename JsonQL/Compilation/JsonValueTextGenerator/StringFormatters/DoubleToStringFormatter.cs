// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;

/// <inheritdoc />
public class DoubleToStringFormatter : IStringFormatter
{
    /// <inheritdoc />
    public bool TryFormat(object value, [NotNullWhen(true)] out string? formattedValue)
    {
        formattedValue = null;

        if (value is not double doubleValue)
            return false;

        formattedValue = doubleValue.ToString(CultureInfo.InvariantCulture);
        return true;
    }
}