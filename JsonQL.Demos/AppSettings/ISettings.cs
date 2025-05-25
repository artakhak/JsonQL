using Microsoft.Extensions.Configuration;

namespace JsonQL.Demos.AppSettings;

public interface ISettings
{
    bool UseCustomJsonQL { get; }
    bool LogDiBasedObjectFactoryDiagnosticsData { get; }
}

public class Settings: ISettings
{
    public Settings(IConfigurationRoot configurationRoot)
    {
        if (bool.TryParse(configurationRoot["Settings:UseCustomJsonQL"], out var useCustomJsonQl))
            UseCustomJsonQL = useCustomJsonQl;

        if (bool.TryParse(configurationRoot["Settings:LogDiBasedObjectFactoryDiagnosticsData"], out var logDiBasedObjectFactoryDiagnosticsData))
            LogDiBasedObjectFactoryDiagnosticsData = logDiBasedObjectFactoryDiagnosticsData;
    }

    public bool UseCustomJsonQL { get; }
    public bool LogDiBasedObjectFactoryDiagnosticsData { get; }
}