namespace JsonQL.Extensions.JsonToObjectConversion;

public interface IConversionErrorsAndWarnings
{
    IConversionErrors ConversionErrors { get; }
    IConversionErrors ConversionWarnings { get; }
}

/// <inheritdoc />
public class ConversionErrorsAndWarnings: IConversionErrorsAndWarnings
{
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