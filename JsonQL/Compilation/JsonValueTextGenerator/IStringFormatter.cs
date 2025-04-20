using System.Diagnostics.CodeAnalysis;

namespace JsonQL.Compilation.JsonValueTextGenerator;

/// <summary>
/// Converts a json value to a formatted <see cref="string"/> value. 
/// </summary>
public interface IStringFormatter
{
    /// <summary>
    /// Converts a json value in <param name="value"></param> to a formatted <see cref="string"/> value.
    /// Some of possible types of values in <param name="value"></param> are <see cref="string"/>, <see cref="double"/>,
    /// <see cref="int"/>, <see cref="bool"/>, <see cref="DateTime"/>, etc. 
    /// </summary>
    /// <param name="value">Value to format.</param>
    /// <param name="formattedValue">Formatted value, if the returned value is true.</param>
    /// <returns>Returns true, if the value was formatted.</returns>
    bool TryFormat(object value, [NotNullWhen(true)]out string? formattedValue);
}