// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Defines a factory interface for creating instances of <see cref="ICalculatedValueJsonValueMutator"/>.
/// </summary>
public interface ICalculatedValueJsonValueMutatorFactory
{
    /// <summary>
    /// Creates an instance of <see cref="ICalculatedValueJsonValueMutator"/> using the provided parsed simple value and JSON function.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value used to initialize the mutator.</param>
    /// <param name="jsonFunction">The JSON function that defines the calculation logic for mutation.</param>
    /// <returns>An instance of <see cref="ICalculatedValueJsonValueMutator"/>.</returns>
    ICalculatedValueJsonValueMutator Create(IParsedSimpleValue parsedSimpleValue, IJsonFunction jsonFunction);
}

/// <inheritdoc />
public class CalculatedValueJsonValueMutatorFactory : ICalculatedValueJsonValueMutatorFactory
{
    private readonly IParsedValueCopy _parsedValueCopy;
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IStringFormatter _stringFormatter;

    /// <summary>
    /// Factory for creating instances of <see cref="ICalculatedValueJsonValueMutator"/> using the specified components for parsed value copying, parsed JSON visiting, and string formatting.
    /// </summary>
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