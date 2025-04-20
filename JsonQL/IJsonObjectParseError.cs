using JsonQL.JsonObjects;

namespace JsonQL;

public interface IJsonObjectParseError
{
    IJsonLineInfo? LineInfo { get; }
    string ErrorMessage { get; }
}

public class JsonObjectParseError : IJsonObjectParseError
{
    public JsonObjectParseError(string errorMessage, IJsonLineInfo? lineInfo)
    {
        ErrorMessage = errorMessage;
        LineInfo = lineInfo;
    }

    public string ErrorMessage { get; }
    public IJsonLineInfo? LineInfo { get; }
}