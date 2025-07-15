// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Represents an abstract base class used to define mutators for JSON values.
/// This class is designed to provide a fundamental structure for creating
/// custom JSON value mutators within the JsonQL framework.
/// </summary>
/// <remarks>
/// Subclasses are expected to provide concrete implementations of the
/// <see cref="Mutate"/> method to define specific mutating logic for JSON values
/// based on the input parameters.
/// </remarks>
public abstract class JsonValueMutatorAbstr : IJsonValueMutator
{
    /// <summary>
    /// Represents an abstract base class for JSON value mutators within the JsonQL
    /// compilation process.
    /// </summary>
    /// <remarks>
    /// This class serves as a foundation for implementing specific mutator logic related to
    /// JSON values. Derived classes are required to implement the <see cref="Mutate"/> method
    /// which defines the mutation behavior for JSON values.
    /// </remarks>
    protected JsonValueMutatorAbstr(IJsonLineInfo? lineInfo)
    {
        LineInfo = lineInfo;
    }

    /// <summary>
    /// Provides line and position information for the relevant JSON element.
    /// This property implements the <see cref="IJsonLineInfo"/> interface and
    /// stores the line number and position within the associated JSON document.
    /// </summary>
    /// <remarks>
    /// This property is often used to associate parsing errors or mutations
    /// with specific locations in a JSON document, aiding in error reporting
    /// and debugging.
    /// </remarks>
    protected IJsonLineInfo? LineInfo { get; }

    /// <inheritdoc/>
    public abstract void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors);
}