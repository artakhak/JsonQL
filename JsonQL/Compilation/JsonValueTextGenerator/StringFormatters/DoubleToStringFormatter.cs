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