using Autofac;
using JsonQL.Demos.AppSettings;
using Microsoft.Extensions.Configuration;

namespace JsonQL.Demos.Startup.DependencyInjection;

public class ConfigurationModule: Module
{
    private readonly ISettings _settings;

    public ConfigurationModule(ISettings settings)
    {
        _settings = settings;
    }

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.Register(x => _settings).As<ISettings>().SingleInstance();
    }
}