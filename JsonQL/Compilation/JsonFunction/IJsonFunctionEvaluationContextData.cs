// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Json function context data.
/// </summary>
public interface IJsonFunctionEvaluationContextData
{
    /// <summary>
    /// Represents the value currently being evaluated within the context of a JSON function evaluation.
    /// This value is used in various computations, such as aggregate operations, where the evaluation logic
    /// depends on each individual item's contribution to an overall result.
    /// </summary>
    IParsedValue EvaluatedValue { get; }

    /// <summary>
    /// Index of <see cref="EvaluatedValue"/> in an evaluated collection.
    /// The collection might be collection of items in <see cref="IParsedArrayValue"/>,
    /// or might be a larger collection that contains a collection or <see cref="IParsedArrayValue"/> (or collection of collections, etc.).
    /// </summary>
    int? Index { get; }
}

/// <inheritdoc />
public class JsonFunctionEvaluationContextData : IJsonFunctionEvaluationContextData
{
    /// <summary>
    /// Provides the context data required for evaluating a JSON function within a given context,
    /// including the parsed value being evaluated and its associated index in a collection.
    /// </summary>
    /// <param name="evaluatedValue">
    /// Represents the value currently being evaluated within the context of a JSON function evaluation.
    /// This value is used in various computations, such as aggregate operations, where the evaluation logic
    /// depends on each individual item's contribution to an overall result.
    /// </param>
    /// <param name="arrayIndex">
    /// Index of <see cref="EvaluatedValue"/> in an evaluated collection.
    /// The collection might be collection of items in <see cref="IParsedArrayValue"/>,
    /// or might be a larger collection that contains a collection or <see cref="IParsedArrayValue"/> (or collection of collections, etc.).
    /// </param>
    public JsonFunctionEvaluationContextData(IParsedValue evaluatedValue, int? arrayIndex)
    {
        EvaluatedValue = evaluatedValue;
        Index = arrayIndex;
    }

    /// <inheritdoc />
    public IParsedValue EvaluatedValue { get; }
    
    /// <inheritdoc />
    public int? Index { get; }
}
