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