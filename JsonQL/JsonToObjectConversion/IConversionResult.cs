using JsonQL.Query;

namespace JsonQL.JsonToObjectConversion;

public interface IConversionResult<out TValue>
{
    TValue? Value { get; }
    IConversionErrorsAndWarnings ConversionErrorsAndWarnings { get; }
}

/// <inheritdoc />
public class ConversionResult<TValue> : IConversionResult<TValue>
{
    public ConversionResult(TValue? value)
    {
        Value = value;
        ConversionErrorsAndWarnings = EmptyErrors.EmptyConversionErrorsAndWarnings;
    }

    public ConversionResult(IConversionErrorsAndWarnings conversionErrorsAndWarnings)
    {
        Value = default;
        ConversionErrorsAndWarnings = conversionErrorsAndWarnings;
    }

    public ConversionResult(TValue? value, IConversionErrorsAndWarnings conversionErrorsAndWarnings)
    {
        Value = value;
        ConversionErrorsAndWarnings = conversionErrorsAndWarnings;
    }

    /// <inheritdoc />
    public TValue? Value { get; }

    public IConversionErrorsAndWarnings ConversionErrorsAndWarnings { get; }
}