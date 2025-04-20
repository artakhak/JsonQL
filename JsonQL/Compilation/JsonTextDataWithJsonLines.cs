namespace JsonQL.Compilation;

public class JsonTextDataWithJsonLines
{
    public JsonTextDataWithJsonLines(IJsonTextData jsonTextData)
    {
        JsonTextData = jsonTextData;
        JsonLines = jsonTextData.JsonText.Split(Environment.NewLine);
    }

    public IJsonTextData JsonTextData { get; }
    public IReadOnlyList<string> JsonLines { get; }
}