namespace JsonQL.Extensions.JsonToObjectConversion;

public interface IJsonConversionSettingsWrapperFactory
{
    IJsonConversionSettingsWrapper Create(IJsonConversionSettings jsonConversionSettings);
}

public class JsonConversionSettingsWrapperFactory : IJsonConversionSettingsWrapperFactory
{
    /// <inheritdoc />
    public IJsonConversionSettingsWrapper Create(IJsonConversionSettings jsonConversionSettings)
    {
        return new JsonConversionSettingsWrapper(jsonConversionSettings);
    }
}