// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an abstract base class for JSON functions that evaluate to a boolean value.
/// </summary>
/// <remarks>
/// This class provides a common framework for implementing JSON functions that return
/// boolean results. It ensures that derived classes provide specific implementations
/// of boolean evaluation logic.
/// </remarks>
public abstract class BooleanJsonFunctionAbstr : JsonFunctionAbstr, IBooleanJsonFunction
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="functionName">Function name.</param>
    /// <param name="jsonFunctionContext">Function context object.</param>
    /// <param name="lineInfo">Function position.</param>
    protected BooleanJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateBooleanValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <inheritdoc />
    public abstract IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}