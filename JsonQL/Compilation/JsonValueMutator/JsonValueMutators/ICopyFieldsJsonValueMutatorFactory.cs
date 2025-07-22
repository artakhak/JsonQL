// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Defines a factory interface for creating instances of <see cref="ICopyFieldsJsonValueMutator"/>.
/// </summary>
public interface ICopyFieldsJsonValueMutatorFactory
{
    /// <summary>
    /// Creates an instance of <see cref="ICopyFieldsJsonValueMutator"/>.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value used for the creation of the JSON value mutator.</param>
    /// <param name="jsonValuePathJsonFunction">The JSON path function applied during the creation of the JSON value mutator.</param>
    /// <returns>An instance of <see cref="ICopyFieldsJsonValueMutator"/> configured using the provided parameters.</returns>
    ICopyFieldsJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonValuePathJsonFunction jsonValuePathJsonFunction);
}

/// <inheritdoc />
public class CopyFieldsJsonValueMutatorFactory : ICopyFieldsJsonValueMutatorFactory
{
    private readonly IParsedValueCopy _parsedValueCopy;

    /// <summary>
    /// Implements the factory interface <see cref="ICopyFieldsJsonValueMutatorFactory"/> to create instances of <see cref="ICopyFieldsJsonValueMutator"/>.
    /// </summary>
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
