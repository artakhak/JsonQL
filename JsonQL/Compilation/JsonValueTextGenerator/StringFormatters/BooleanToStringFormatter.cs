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