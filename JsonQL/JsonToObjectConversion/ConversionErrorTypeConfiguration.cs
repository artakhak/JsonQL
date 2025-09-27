namespace JsonQL.JsonToObjectConversion;

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