using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonFunction;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

public interface ICalculatedValueJsonValueMutatorFactory
{
    ICalculatedValueJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonFunction jsonFunction);
}

/// <inheritdoc />
public class CalculatedValueJsonValueMutatorFactory : ICalculatedValueJsonValueMutatorFactory
{
    private readonly IParsedValueCopy _parsedValueCopy;
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IStringFormatter _stringFormatter;

    public CalculatedValueJsonValueMutatorFactory(IParsedValueCopy parsedValueCopy,
        IParsedJsonVisitor parsedJsonVisitor, IStringFormatter stringFormatter)
    {
        _parsedValueCopy = parsedValueCopy;
        _parsedJsonVisitor = parsedJsonVisitor;
        _stringFormatter = stringFormatter;
    }

    /// <inheritdoc />
    public ICalculatedValueJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonFunction jsonFunction)
    {
        return new CalculatedValueJsonValueMutator(parsedSimpleValue, jsonFunction,
            _parsedValueCopy, _parsedJsonVisitor, _stringFormatter);
    }
}