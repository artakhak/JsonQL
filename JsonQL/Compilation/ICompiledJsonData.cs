using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

public interface ICompiledJsonData
{
    string TextIdentifier { get; }
    string JsonText { get; }
    IRootParsedValue CompiledParsedValue { get; }
}

public class CompiledJsonData : ICompiledJsonData
{
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