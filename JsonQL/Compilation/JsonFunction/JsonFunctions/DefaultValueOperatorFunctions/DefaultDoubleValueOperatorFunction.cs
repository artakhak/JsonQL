// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

/// <summary>
/// Represents a JSON function that evaluates to a double value with a default value fallback.
/// </summary>
/// <remarks>
/// This class extends the <see cref="DefaultValueOperatorFunction"/> to provide functionality for handling double-type JSON operations
/// where a main and a default value are evaluated in the context of the JSON function. If the main value is not available or invalid,
/// the default value is used as the fallback.
/// </remarks>
public class DefaultDoubleValueOperatorFunction : DefaultValueOperatorFunction, IDoubleJsonFunction
{
    /// <summary>
    /// Represents a default value operator function for double values in JsonQL.
    /// Offers the capability to evaluate a primary double value against a default double value
    /// within a given JSON function context, and returns the default value if the primary value is deemed invalid.
    /// </summary>
    /// <param name="operatorName">The name of the operator associated with this function.</param>
    /// <param name="mainValueJsonFunction">The function representing the main value to be evaluated.</param>
    /// <param name="defaultValueJsonFunction">The function representing the default value to return when the main value is invalid.</param>
    /// <param name="jsonFunctionContext">The context used for evaluating the JSON function values.</param>
    /// <param name="lineInfo">Optional line information providing context for error handling or debugging.</param>
    public DefaultDoubleValueOperatorFunction(string operatorName, IDoubleJsonFunction mainValueJsonFunction, IDoubleJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(LineInfo);
    }
}