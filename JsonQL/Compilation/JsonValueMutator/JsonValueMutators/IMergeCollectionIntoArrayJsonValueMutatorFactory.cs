using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

public interface IMergeCollectionIntoArrayJsonValueMutatorFactory
{
    IMergeCollectionIntoArrayJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonFunction jsonFunction);
}

/// <inheritdoc />
public class MergeCollectionIntoArrayJsonValueMutatorFactory : IMergeCollectionIntoArrayJsonValueMutatorFactory
{
    private readonly IParsedValueCopy _parsedValueCopy;
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IStringFormatter _stringFormatter;

    public MergeCollectionIntoArrayJsonValueMutatorFactory(IParsedValueCopy parsedValueCopy,
        IParsedJsonVisitor parsedJsonVisitor, IStringFormatter stringFormatter)
    {
        _parsedValueCopy = parsedValueCopy;
        _parsedJsonVisitor = parsedJsonVisitor;
        _stringFormatter = stringFormatter;
    }

    /// <inheritdoc />
    public IMergeCollectionIntoArrayJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue,
        IJsonFunction jsonFunction)
    {
        return new MergeCollectionIntoArrayJsonValueMutator(parsedSimpleValue,
            jsonFunction, _parsedValueCopy, _parsedJsonVisitor, _stringFormatter);
    }
}