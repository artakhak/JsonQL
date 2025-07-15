// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents a JSON function specifically designed to process and evaluate boolean values.
/// </summary>
public interface IBooleanJsonFunction : IJsonFunction
{
    /// <summary>
    /// Evaluates the provided parsed values and context to produce a boolean parse result.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value to be evaluated.</param>
    /// <param name="compiledParentRootParsedValues">A read-only list of compiled parent root parsed values that may influence the evaluation.</param>
    /// <param name="contextData">Optional context data to assist in the evaluation.</param>
    /// <returns>A parse result containing a nullable boolean value resulting from the evaluation.</returns>
    IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}