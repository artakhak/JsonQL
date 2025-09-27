namespace JsonQL.Compilation;

/// <inheritdoc />
public class JsonTextData: IJsonTextData
{
    /// <summary>
    /// Represents JSON text data and allows for hierarchical structuring of JSON text.
    /// Used across the JSON compilation and query processes for identifying and handling JSON text.
    /// This class provides:
    /// - The ability to store the raw JSON text content.
    /// - Identification through a unique text identifier.
    /// - Linking to a parent JSON text data instance for hierarchical relationships.
    /// </summary>
    public JsonTextData(string textIdentifier, string jsonText, IJsonTextData? parentJsonTextData = null)
    {
        TextIdentifier = textIdentifier;
        JsonText = jsonText;
        ParentJsonTextData = parentJsonTextData;
    }

    /// <inheritdoc />
    public string TextIdentifier { get; }
    
    /// <inheritdoc />
    public string JsonText { get; }
    
    /// <inheritdoc />
    public IJsonTextData? ParentJsonTextData { get; }
}