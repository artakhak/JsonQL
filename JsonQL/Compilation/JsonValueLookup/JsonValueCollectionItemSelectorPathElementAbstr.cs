using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents an abstract base class for JSON value collection item selector path elements.
/// This class defines the core functionality for selecting an item from a collection within
/// a JSON structure during path lookup execution.
/// </summary>
public abstract class JsonValueCollectionItemSelectorPathElementAbstr : IJsonValueCollectionItemsSelectorPathElement
{
    /// <summary>
    /// Serves as the base class for all path elements that implement the logic
    /// for selecting items from JSON value collections during path evaluation.
    /// </summary>
    protected JsonValueCollectionItemSelectorPathElementAbstr(string functionName, IJsonLineInfo? lineInfo)
    {
        FunctionName = functionName;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> Select(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        return SelectCollectionItem(parenParsedValues, rootParsedValue, compiledParentRootParsedValues);
    }

    /// <summary>
    /// Selects a specific item from a JSON value collection based on the provided parent parsed values,
    /// root parsed value, and compiled parent root parsed values.
    /// This method must be implemented by derived classes to provide the logic for selecting the appropriate item.
    /// </summary>
    /// <param name="parenParsedValues">A read-only list of parent parsed values used in the selection process.</param>
    /// <param name="rootParsedValue">The root parsed value for the JSON data being evaluated.</param>
    /// <param name="compiledParentRootParsedValues">A read-only list of compiled parent root parsed values to assist in the selection process.</param>
    /// <returns>An instance of <see cref="IParseResult{TValue}"/> that contains the result of the selection as an
    /// <see cref="ISingleItemJsonValuePathLookupResult"/>.</returns>
    protected abstract IParseResult<ISingleItemJsonValuePathLookupResult> SelectCollectionItem(IReadOnlyList<IParsedValue> parenParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);

    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc />
    public string FunctionName { get; }

    /// <inheritdoc />
    public bool SelectsSingleItem => true;

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{FunctionName}(...)";
    }
}