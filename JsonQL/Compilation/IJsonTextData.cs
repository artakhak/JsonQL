namespace JsonQL.Compilation;

/// <summary>
/// Information about Json text.
/// </summary>
public interface IJsonTextData
{
    /// <summary>
    /// Unique identifier. Example: "File1.Json" or "549B9915-9768-4DC4-BE15-B17A394ED7B3"
    /// </summary>
    string TextIdentifier { get; }
    string JsonText { get; }
    IJsonTextData? ParentJsonTextData { get; }
}

public class JsonTextData: IJsonTextData
{
    public JsonTextData(string textIdentifier, string jsonText, IJsonTextData? parentJsonTextData = null)
    {
        TextIdentifier = textIdentifier;
        JsonText = jsonText;
        ParentJsonTextData = parentJsonTextData;
    }

    public string TextIdentifier { get; }
    public string JsonText { get; }
    public IJsonTextData? ParentJsonTextData { get; }
}