using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

/// <inheritdoc />
public class CompiledJsonData : ICompiledJsonData
{
    /// <summary>
    /// Represents a compiled JSON data object that contains the text identifier,
    /// the JSON text, and the corresponding compiled parsed value.
    /// </summary>
    public CompiledJsonData(string textIdentifier, string jsonText, IRootParsedValue compiledParsedValue)
    {
        TextIdentifier = textIdentifier;
        CompiledParsedValue = compiledParsedValue;
        JsonText = jsonText;
    }

    /// <inheritdoc />
    public string TextIdentifier { get; }

    /// <inheritdoc />
    public string JsonText { get; }

    /// <inheritdoc />
    public IRootParsedValue CompiledParsedValue { get; }
}