using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

public interface IJsonObjectData
{
    IJsonTextData JsonTextData { get; }

    /// <summary>
    /// Parsed value. An instance of either <see cref="IRootParsedJson"/> if the compiled object is json object,
    /// or <see cref="IRootParsedArrayValue"/> if compiled json is an array.
    /// </summary>
    IRootParsedValue RootParsedValue { get; }
    IJsonObjectData? ParentJsonObjectData { get; }
}

public class JsonObjectData: IJsonObjectData
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonTextData"></param>
    /// <param name="rootParsedValue">
    /// Parsed value. An instance of either <see cref="IRootParsedJson"/> if the compiled object is json object,
    /// or <see cref="IRootParsedArrayValue"/> if compiled json is an array.
    /// </param>
    /// <param name="parentJsonObjectData"></param>
    public JsonObjectData(IJsonTextData jsonTextData, IRootParsedValue rootParsedValue, IJsonObjectData? parentJsonObjectData = null)
    {
        JsonTextData = jsonTextData;
        RootParsedValue = rootParsedValue;
        ParentJsonObjectData = parentJsonObjectData;
    }

    public IJsonTextData JsonTextData { get; }
    public IRootParsedValue RootParsedValue { get; }
    public IJsonObjectData? ParentJsonObjectData { get; }
}