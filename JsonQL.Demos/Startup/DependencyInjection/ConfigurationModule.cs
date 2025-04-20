using Autofac;
using JsonQL.Demos.AppSettings;
using Microsoft.Extensions.Configuration;

namespace JsonQL.Demos.Startup.DependencyInjection;

public class ConfigurationModule: Module
{
    private readonly IConfigurationRoot _configurationRoot;

    public ConfigurationModule(IConfigurationRoot configurationRoot)
    {
        _configurationRoot = configurationRoot;
    }

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        var settings = new Settings();

        if (bool.TryParse(_configurationRoot["Settings:UseCustomJsonQL"], out var useCustomJsonQl) && useCustomJsonQl)
            settings.UseCustomJsonQL = true;
      
        builder.Register(x => new AppSettings.AppSettings(settings)).As<IAppSettings>();
    }
}