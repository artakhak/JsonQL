namespace JsonQL.Demos.AppSettings;

public interface ISettings
{
    bool UseCustomJsonQL { get; }
}

public class Settings: ISettings
{
    public bool UseCustomJsonQL { get; set; }
}