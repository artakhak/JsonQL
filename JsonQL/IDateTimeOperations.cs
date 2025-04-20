using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace JsonQL;

public interface IDateTimeOperations
{
    bool TryParse(string dateTimeText, [NotNullWhen(true)] out DateTime? dateTime);
    string ToString(DateTime dateTime);
    DateTime ConvertToDate(DateTime dateTime);
}

/// <inheritdoc />
public class DateTimeOperations : IDateTimeOperations
{
    private const string OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff";

    private static readonly List<string> DateTimeFormats = new()
    {
        "yyyy-MM-dd HH:mm:ss.fffffff",
        "yyyy/MM/dd HH:mm:ss.fffffff",
        "MM-dd-yyyy HH:mm:ss.fffffff",
        "MM/dd/yyyy HH:mm:ss.fffffff",
        "yyyy-MM-dd HH:mm:ss",
        "MM-dd-yyyy HH:mm:ss",
        "yyyy-MM-dd",
        "yyyy/MM/dd",
        "MM-dd-yyyy",
        "MM/dd/yyyy"
    };

    /// <inheritdoc />
    public bool TryParse(string dateTimeText, [NotNullWhen(true)] out DateTime? dateTime)
    {
        if (DateTime.TryParse(dateTimeText, out var parsedDateTime))
        {
            dateTime = parsedDateTime;
            return true;
        }

        foreach (var dateTimeFormat in DateTimeFormats)
        {
            if (DateTime.TryParseExact(dateTimeText, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
            {
                dateTime = parsedDateTime;
                return true;
            }
        }
        
        dateTime = null;
        return false;

    }

    /// <inheritdoc />
    public string ToString(DateTime dateTime)
    {
        return dateTime.ToString(OutputDateTimeFormat);
    }

    /// <inheritdoc />
    public DateTime ConvertToDate(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
    }
}