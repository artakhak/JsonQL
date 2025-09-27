namespace JsonQL.JsonToObjectConversion;

/// <inheritdoc />
public class JsonConversionSettingsOverrides : IJsonConversionSettingsOverrides
{
    /// <inheritdoc />
    public JsonPropertyFormat? JsonPropertyFormat { get; set; }

    /// <inheritdoc />
    public bool? FailOnFirstError { get; set; }

    /// <inheritdoc />
    public TryMapTypeDelegate? TryMapJsonConversionType { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<IConversionErrorTypeConfiguration>? ConversionErrorTypeConfigurations { get; set; } = null;
}