// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace JsonQL;

/// <summary>
/// Provides operations for parsing, formatting, and converting date and time values.
/// </summary>
public interface IDateTimeOperations
{
    /// <summary>
    /// Attempts to parse the input string representation of a date and time
    /// and converts it to a <see cref="DateTime"/> object if parsing succeeds.
    /// </summary>
    /// <param name="dateTimeText">
    /// The string containing the date and time to parse.
    /// </param>
    /// <param name="dateTime">
    /// When this method returns <c>true</c>, contains the parsed <see cref="DateTime"/> value.
    /// If parsing fails, this parameter contains <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the parsing was successful; otherwise, <c>false</c>.
    /// </returns>
    bool TryParse(string dateTimeText, [NotNullWhen(true)] out DateTime? dateTime);

    /// <summary>
    /// Converts the specified <see cref="DateTime"/> object to its string representation
    /// using the configured date and time format.
    /// </summary>
    /// <param name="dateTime">
    /// The <see cref="DateTime"/> object to be converted to a string.
    /// </param>
    /// <returns>
    /// A string representation of the specified <see cref="DateTime"/> object.
    /// </returns>
    string ToString(DateTime dateTime);

    /// <summary>
    /// Converts the specified <see cref="DateTime"/> value to a new <see cref="DateTime"/>
    /// with its time component set to 00:00:00 (midnight) in UTC.
    /// </summary>
    /// <param name="dateTime">
    /// The <see cref="DateTime"/> value to convert.
    /// </param>
    /// <returns>
    /// A <see cref="DateTime"/> instance representing the same date as the input
    /// with the time component set to 00:00:00 (midnight) in UTC.
    /// </returns>
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