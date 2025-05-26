// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

/// <summary>
/// Represents a default value operator function specifically for DateTime values.
/// </summary>
/// <remarks>
/// This class allows the evaluation of a primary DateTime value expression,
/// and if it results in invalid or null data, a default DateTime value is applied.
/// </remarks>
public class DefaultDateTimeValueOperatorFunction : DefaultValueOperatorFunction, IDateTimeJsonFunction
{
    /// <summary>
    /// A function that evaluates JSON date and time functions, allowing specification of a default value fallback if the primary value is unavailable.
    /// </summary>
    /// <param name="operatorName">The name of the operator used in the function evaluation.</param>
    /// <param name="mainValueJsonFunction">The primary JSON function for evaluating the date and time value.</param>
    /// <param name="defaultValueJsonFunction">The JSON function supplying a default value if the primary value is not available.</param>
    /// <param name="jsonFunctionContext">The context required to evaluate the JSON function.</param>
    /// <param name="lineInfo">Optional line information used for debugging or error tracing.</param>
    public DefaultDateTimeValueOperatorFunction(string operatorName, IDateTimeJsonFunction mainValueJsonFunction, IDateTimeJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<DateTime?> EvaluateDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDateTime(LineInfo);
    }
}