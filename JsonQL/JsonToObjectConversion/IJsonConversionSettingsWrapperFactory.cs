namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Provides a factory interface for creating instances of <see cref="IJsonConversionSettingsWrapper"/>.
/// </summary>
public interface IJsonConversionSettingsWrapperFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IJsonConversionSettingsWrapper"/> using the provided JSON conversion settings.
    /// </summary>
    /// <param name="jsonConversionSettings">The configuration settings for JSON conversion.</param>
    /// <returns>An instance of <see cref="IJsonConversionSettingsWrapper"/>.</returns>
    IJsonConversionSettingsWrapper Create(IJsonConversionSettings jsonConversionSettings);
}

/// <inheritdoc />
public class JsonConversionSettingsWrapperFactory : IJsonConversionSettingsWrapperFactory
{
    /// <inheritdoc />
    public IJsonConversionSettingsWrapper Create(IJsonConversionSettings jsonConversionSettings)
    {
        return new JsonConversionSettingsWrapper(jsonConversionSettings);
    }
}