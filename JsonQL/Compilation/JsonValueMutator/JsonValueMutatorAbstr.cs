using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Base class for <see cref="IJsonValueMutator"/>
/// </summary>
public abstract class JsonValueMutatorAbstr : IJsonValueMutator
{
    protected JsonValueMutatorAbstr(IJsonLineInfo? lineInfo)
    {
        LineInfo = lineInfo;
    }

    protected IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc/>
    public abstract void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors);
}