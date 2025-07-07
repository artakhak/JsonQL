// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Represents a factory for creating instances of <see cref="IMergeCollectionIntoArrayJsonValueMutator"/>.
/// </summary>
public interface IMergeCollectionIntoArrayJsonValueMutatorFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IMergeCollectionIntoArrayJsonValueMutator"/> based on the provided parameters.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value to be used in the creation process.</param>
    /// <param name="jsonFunction">The JSON function to be applied during the mutation process.</param>
    /// <returns>An instance of <see cref="IMergeCollectionIntoArrayJsonValueMutator"/>.</returns>
    IMergeCollectionIntoArrayJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonFunction jsonFunction);
}

/// <inheritdoc />
public class MergeCollectionIntoArrayJsonValueMutatorFactory : IMergeCollectionIntoArrayJsonValueMutatorFactory
{
    private readonly IParsedValueCopy _parsedValueCopy;
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IStringFormatter _stringFormatter;

    /// <summary>
    /// Factory for creating instances of <see cref="IMergeCollectionIntoArrayJsonValueMutator"/> with required dependencies.
    /// </summary>
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