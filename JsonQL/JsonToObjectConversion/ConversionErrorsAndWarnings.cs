namespace JsonQL.JsonToObjectConversion;

/// <inheritdoc />
public class ConversionErrorsAndWarnings: IConversionErrorsAndWarnings
{
    /// <summary>
    /// Represents a class designed to hold instances of errors and warnings
    /// encountered during JSON to object conversion processes.
    /// </summary>
    public ConversionErrorsAndWarnings(IConversionErrors conversionErrors, IConversionErrors conversionWarnings)
    {
        ConversionErrors = conversionErrors;
        ConversionWarnings = conversionWarnings;
    }

    /// <inheritdoc />
    public IConversionErrors ConversionErrors { get; }

    /// <inheritdoc />
    public IConversionErrors ConversionWarnings { get; }
}