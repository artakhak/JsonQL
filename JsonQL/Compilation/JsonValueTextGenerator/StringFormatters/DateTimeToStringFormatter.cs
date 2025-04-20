using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace JsonQL.Compilation.JsonValueTextGenerator.StringFormatters;

/// <inheritdoc />
public class DateTimeToStringFormatter : IStringFormatter
{
    private readonly IDateTimeOperations _dateTimeOperations;

    public DateTimeToStringFormatter(IDateTimeOperations dateTimeOperations)
    {
        _dateTimeOperations = dateTimeOperations;
    }

    /// <inheritdoc />
    public bool TryFormat(object value, [NotNullWhen(true)] out string? formattedValue)
    {
        formattedValue = null;

        if (value is not DateTime dateTime)
            return false;

        formattedValue = _dateTimeOperations.ToString(dateTime);
        return true;
    }
}

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