namespace JsonQL.Demos.AppSettings;

public interface IAppSettings
{
    ISettings Settings { get; }
}

public class AppSettings: IAppSettings
{
    public AppSettings(ISettings settings)
    {
        Settings = settings;
    }

    public ISettings Settings { get; }
}