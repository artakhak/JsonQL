namespace JsonQL.JsonToObjectConversion;

/// <inheritdoc />
public class JsonConversionSettings : IJsonConversionSettings
{
    /// <inheritdoc />
    public JsonPropertyFormat JsonPropertyFormat { get; set; } = JsonPropertyFormat.PascalCase;

    /// <inheritdoc />
    public TryMapTypeDelegate? TryMapJsonConversionType { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<IConversionErrorTypeConfiguration> ConversionErrorTypeConfigurations { get; set; } = 
        Array.Empty<IConversionErrorTypeConfiguration>();

    /// <inheritdoc />
    public bool FailOnFirstError { get; set; } = true;
}