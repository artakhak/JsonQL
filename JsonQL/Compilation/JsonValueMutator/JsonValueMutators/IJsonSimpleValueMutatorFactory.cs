// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Defines a factory for creating instances of <see cref="IJsonSimpleValueMutator" />.
/// </summary>
public interface IJsonSimpleValueMutatorFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="IJsonSimpleValueMutator"/>.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value to be used by the mutator.</param>
    /// <param name="parsedTextGenerators">A list of text generators for converting expressions to strings.</param>
    /// <param name="parsedValueTemplate">The template for formatting the parsed value.</param>
    /// <returns>A new instance of <see cref="IJsonSimpleValueMutator"/>.</returns>
    IJsonSimpleValueMutator Create(IParsedSimpleValue parsedSimpleValue,
        IReadOnlyList<IJsonSimpleValueExpressionToStringConverter> parsedTextGenerators, string parsedValueTemplate);
}

/// <inheritdoc />
public class JsonSimpleValueMutatorFactory : IJsonSimpleValueMutatorFactory
{
    /// <inheritdoc />
    public IJsonSimpleValueMutator Create(IParsedSimpleValue parsedSimpleValue,
        IReadOnlyList<IJsonSimpleValueExpressionToStringConverter> parsedTextGenerators, string parsedValueTemplate)
    {
        return new JsonSimpleValueMutator(parsedSimpleValue, parsedTextGenerators, parsedValueTemplate);
    }
}