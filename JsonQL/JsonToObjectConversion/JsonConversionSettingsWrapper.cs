using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonToObjectConversion;

public interface IJsonConversionSettingsWrapper
{ 
    IJsonConversionSettings JsonConversionSettings { get; }
    bool TryGetConversionErrorTypeConfiguration(ConversionErrorType conversionErrorType, [NotNullWhen(true)]out IConversionErrorTypeConfiguration? conversionErrorTypeConfiguration);
}

public class JsonConversionSettingsWrapper : IJsonConversionSettingsWrapper
{
    private readonly Dictionary<ConversionErrorType, IConversionErrorTypeConfiguration> _conversionErrorTypeToConfigurationMap = new();
    
    public JsonConversionSettingsWrapper(IJsonConversionSettings jsonConversionSettings)
    {
        JsonConversionSettings = jsonConversionSettings;

        foreach (var conversionErrorTypeConfiguration in jsonConversionSettings.ConversionErrorTypeConfigurations)
        {
            _conversionErrorTypeToConfigurationMap[conversionErrorTypeConfiguration.ConversionErrorType] = conversionErrorTypeConfiguration;
        }
    }

    /// <inheritdoc />
    public IJsonConversionSettings JsonConversionSettings { get; }

    /// <inheritdoc />
    public bool TryGetConversionErrorTypeConfiguration(ConversionErrorType conversionErrorType, [NotNullWhen(true)] out IConversionErrorTypeConfiguration? conversionErrorTypeConfiguration)
    {
        return _conversionErrorTypeToConfigurationMap.TryGetValue(conversionErrorType, out conversionErrorTypeConfiguration);
    }
}

