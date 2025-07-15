// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

namespace JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;

public class AggregatedStringFormatter : IStringFormatter
{
    private readonly IReadOnlyList<IStringFormatter> _formatters;

    public AggregatedStringFormatter(IReadOnlyList<IStringFormatter> formatters)
    {
        _formatters = formatters;
    }
   
    public bool TryFormat(object value, [NotNullWhen(true)] out string? formattedValue)
    {
        formattedValue = null;

        foreach (var formatter in _formatters)
        {
            if (formatter.TryFormat(value, out formattedValue))
                return true;
        }

        return false;
    }
}