using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

public interface ICompiledJsonData
{
    string TextIdentifier { get; }
    IRootParsedValue CompiledParsedValue { get; }
}

public class CompiledJsonData : ICompiledJsonData
{
    public CompiledJsonData(string textIdentifier, IRootParsedValue compiledParsedValue)
    {
        TextIdentifier = textIdentifier;
        CompiledParsedValue = compiledParsedValue;
    }

    /// <inheritdoc />
    public string TextIdentifier { get; }

    /// <inheritdoc />
    public IRootParsedValue CompiledParsedValue { get; }
}