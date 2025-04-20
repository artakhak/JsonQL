using JsonQL.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

public interface ICopyFieldsJsonValueMutatorFactory
{
    ICopyFieldsJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonValuePathJsonFunction jsonValuePathJsonFunction);
}

public class CopyFieldsJsonValueMutatorFactory : ICopyFieldsJsonValueMutatorFactory
{
    private readonly IParsedValueCopy _parsedValueCopy;

    public CopyFieldsJsonValueMutatorFactory(IParsedValueCopy parsedValueCopy)
    {
        _parsedValueCopy = parsedValueCopy;
    }

    /// <inheritdoc />
    public ICopyFieldsJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonValuePathJsonFunction jsonValuePathJsonFunction)
    {
        return new CopyFieldsJsonValueMutator(parsedSimpleValue, jsonValuePathJsonFunction, _parsedValueCopy);
    }
}