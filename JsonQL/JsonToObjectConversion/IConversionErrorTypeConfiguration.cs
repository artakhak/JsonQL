namespace JsonQL.JsonToObjectConversion;

public interface IConversionErrorTypeConfiguration
{
    ConversionErrorType ConversionErrorType { get; }
    ErrorReportingType ErrorReportingType { get; }
}

/// <inheritdoc />
public class ConversionErrorTypeConfiguration : IConversionErrorTypeConfiguration
{
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