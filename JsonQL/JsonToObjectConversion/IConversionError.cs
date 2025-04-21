using System.Collections.Generic;
using JsonQL.JsonObjects;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Conversion error details.
/// </summary>
public interface IConversionError
{
    /// <summary>
    /// Error type.
    /// </summary>
    ConversionErrorType ErrorType { get; }

    /// <summary>
    /// If the value is not null, parsed json value associated with error.
    /// </summary>
    IParsedValue? ParsedValue { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    string Error { get; }

    /// <summary>
    /// If the value is not null, path describing the object path.
    /// Example: ["Employees, "[0]", "Address", "Street"]
    /// </summary>
    IReadOnlyList<string>? ObjectPath { get; }
}

/// <inheritdoc />
public class ConversionError : IConversionError
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorType">Error type.</param>
    /// <param name="error">Error message.</param>
    /// <param name="objectPath">
    /// If the value is not null, path describing the object path.
    /// Example: ["Employees, "[0]", "Address", "Street"]
    /// </param>
    /// <param name="parsedValue">If the value is not null, parsed json value associated with error.</param>
    public ConversionError(ConversionErrorType errorType, string error, IReadOnlyList<string> objectPath, IParsedValue? parsedValue)
    {
        ErrorType = errorType;
        Error = error;
        ObjectPath = objectPath;
        ParsedValue = parsedValue;
    }

    /// <inheritdoc />
    public ConversionErrorType ErrorType { get; }

    /// <inheritdoc />
    public IParsedValue? ParsedValue { get; }

    /// <inheritdoc />
    public string Error { get; }

    /// <inheritdoc />
    public IReadOnlyList<string>? ObjectPath { get; }
}