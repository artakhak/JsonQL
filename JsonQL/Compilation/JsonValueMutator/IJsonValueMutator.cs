// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Defines the contract for mutating JSON values during compilation.
/// Implementations of this interface modify a JSON value and may generate errors
/// that occur during the mutation process.
/// </summary>
public interface IJsonValueMutator
{
    /// <summary>
    /// Mutates the root JSON value and generates errors that occur during the mutation process.
    /// </summary>
    /// <param name="rootParsedValue">
    /// The root-parsed JSON value to be mutated.
    /// </param>
    /// <param name="compiledParentRootParsedValues">
    /// A collection of already compiled root JSON values that may influence the mutation process.
    /// </param>
    /// <param name="errors">
    /// A list to which any errors encountered during the mutation will be added.
    /// </param>
    void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors);
}
