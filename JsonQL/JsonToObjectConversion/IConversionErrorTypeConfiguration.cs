namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents the configuration for a specific type of conversion error in the JSON-to-object conversion process.
/// </summary>
public interface IConversionErrorTypeConfiguration
{
    /// <summary>
    /// Defines the types of conversion errors that can occur during the JSON-to-object conversion process.
    /// </summary>
    ConversionErrorType ConversionErrorType { get; }

    /// <summary>
    /// Specifies how conversion errors are handled during the JSON-to-object conversion process.
    /// </summary>
    ErrorReportingType ErrorReportingType { get; }
}

/// <inheritdoc />
public class ConversionErrorTypeConfiguration : IConversionErrorTypeConfiguration
{
    /// <summary>
    /// Represents the configuration for a specific type of conversion error in the JSON-to-object conversion process.
    /// </summary>
    public ConversionErrorTypeConfiguration(ConversionErrorType conversionErrorType, ErrorReportingType errorReportingType)
    {
        ConversionErrorType = conversionErrorType;
        ErrorReportingType = errorReportingType;
    }

    /// <inheritdoc />
    public ConversionErrorType ConversionErrorType { get; }

    /// <inheritdoc />
    public ErrorReportingType ErrorReportingType { get; }
}