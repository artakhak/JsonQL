using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

public interface IJsonValueMutator
{
    /// <summary>
    /// Mutates the json value in <param name="rootParsedValue"></param>
    /// </summary>
    /// <param name="rootParsedValue">Mutated json value.</param>
    /// <param name="compiledParentRootParsedValues">Parent json values.</param>
    /// <param name="errors">Errors reported during the mutation. The implementation should add errors to this list.</param>
    void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors);
}