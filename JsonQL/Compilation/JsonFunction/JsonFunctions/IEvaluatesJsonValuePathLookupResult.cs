// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents an interface for evaluating JSON value path lookup results within a given context.
/// This interface provides the functionality to process JSON value path queries and return corresponding results.
/// </summary>
public interface IEvaluatesJsonValuePathLookupResult: IJsonFunction
{
    /// <summary>
    /// Evaluates the JSON value path function based on the provided parsed values and evaluation context.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value to evaluate.</param>
    /// <param name="compiledParentRootParsedValues">A collection of compiled parent root parsed values required for evaluation.</param>
    /// <param name="contextData">The optional context data used during the evaluation process.</param>
    /// <returns>A parse result containing the lookup result for the JSON value path.</returns>
    IParseResult<IJsonValuePathLookupResult> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData);
}