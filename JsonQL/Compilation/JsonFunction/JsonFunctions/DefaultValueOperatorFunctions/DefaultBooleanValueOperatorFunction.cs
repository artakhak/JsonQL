// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

/// <summary>
/// Represents a specific implementation of a default value operator function that operates on boolean values.
/// </summary>
/// <remarks>
/// This class is an extension of the <see cref="DefaultValueOperatorFunction"/> and is specifically tailored to handle
/// boolean values within JSON function processing. It implements the <see cref="IBooleanJsonFunction"/> interface
/// to ensure compatibility with other boolean-specific JSON functions.
/// </remarks>
/// <seealso cref="DefaultValueOperatorFunction"/>
/// <seealso cref="IBooleanJsonFunction"/>
public class DefaultBooleanValueOperatorFunction : DefaultValueOperatorFunction, IBooleanJsonFunction
{
    /// <summary>
    /// Represents a default value operator function specifically for boolean values.
    /// This implementation evaluates a primary boolean function and a default boolean function within the given context, producing the appropriate result based on the evaluation.
    /// </summary>
    /// <param name="operatorName">The name of the operator.</param>
    /// <param name="mainValueJsonFunction">The primary boolean function to evaluate.</param>
    /// <param name="defaultValueJsonFunction">The default boolean function to be used if the primary evaluation is null or invalid.</param>
    /// <param name="jsonFunctionContext">The context in which the function evaluation takes place.</param>
    /// <param name="lineInfo">Optional metadata related to the JSON line for debugging or diagnostic purposes.</param>
    public DefaultBooleanValueOperatorFunction(string operatorName, IBooleanJsonFunction mainValueJsonFunction, IBooleanJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToBoolean(LineInfo);
    }
}