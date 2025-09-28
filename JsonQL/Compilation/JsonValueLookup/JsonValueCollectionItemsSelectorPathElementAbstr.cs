// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Represents an abstract base class for path elements used to select items from a collection
/// in a JSON value lookup operation. This class provides a framework for implementing custom
/// collection item selection behaviors in derived classes.
/// </summary>
public abstract class JsonValueCollectionItemsSelectorPathElementAbstr : IJsonValueCollectionItemsSelectorPathElement
{
    /// <summary>
    /// Serves as an abstract base class for path elements that provide functionality
    /// to select items from a collection during JSON value lookup operations.
    /// Derived classes implement specific item selection logic tailored to
    /// their use case.
    /// </summary>
    protected JsonValueCollectionItemsSelectorPathElementAbstr(string functionName, IJsonLineInfo? lineInfo)
    {
        FunctionName = functionName;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> Select(IReadOnlyList<IParsedValue> parentParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        return SelectCollectionItems(parentParsedValues, rootParsedValue, compiledParentRootParsedValues);
    }

    /// <summary>
    /// Selects and resolves items from a collection based on the provided parent parsed values,
    /// root parsed value, and compiled parent root parsed values. The implementation determines
    /// how the item selection is performed to support specific path element logic.
    /// </summary>
    /// <param name="parentParsedValues">The list of parsed values representing the parent's context for item selection.</param>
    /// <param name="rootParsedValue">The root parsed value serving as the context for the collection items.</param>
    /// <param name="compiledParentRootParsedValues">The compiled list of root parsed values providing extended context for item resolution.</param>
    /// <returns>A parse result containing the resolved collection of JSON value path lookup results.</returns>
    protected abstract IParseResult<ICollectionJsonValuePathLookupResult> SelectCollectionItems(IReadOnlyList<IParsedValue> parentParsedValues, IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
    
    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc />
    public string FunctionName { get; }

    /// <inheritdoc />
    public bool SelectsSingleItem => false;
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"{FunctionName}(...)";
    }
}
