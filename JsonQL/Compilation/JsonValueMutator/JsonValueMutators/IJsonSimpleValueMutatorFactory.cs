using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

public interface IJsonSimpleValueMutatorFactory
{
    IJsonSimpleValueMutator Create(IParsedSimpleValue parsedSimpleValue,
        IReadOnlyList<IJsonSimpleValueExpressionToStringConverter> parsedTextGenerators, string parsedValueTemplate);
}

/// <inheritdoc />
public class JsonSimpleValueMutatorFactory : IJsonSimpleValueMutatorFactory
{
    /// <inheritdoc />
    public IJsonSimpleValueMutator Create(IParsedSimpleValue parsedSimpleValue,
        IReadOnlyList<IJsonSimpleValueExpressionToStringConverter> parsedTextGenerators, string parsedValueTemplate)
    {
        return new JsonSimpleValueMutator(parsedSimpleValue, parsedTextGenerators, parsedValueTemplate);
    }
}