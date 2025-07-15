// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Abstract base class for DateTime-based JSON function implementations.
/// Provides common functionality for evaluating and extracting DateTime values
/// from JSON data by implementing and extending functionality defined in JsonFunctionAbstr.
/// </summary>
public abstract class DateTimeJsonFunctionAbstr : JsonFunctionAbstr, IDateTimeJsonFunction
{
    /// <summary>
    /// Serves as an abstract base class for JSON functions that handle DateTime operations.
    /// </summary>
    /// <remarks>
    /// This class defines the foundation for implementing specific DateTime-related JSON functions.
    /// It provides a mechanism to evaluate DateTime values and enforces derived classes to implement
    /// the logic for retrieving a valid DateTime value.
    /// </remarks>
    protected DateTimeJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {

    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateDateTimeValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <inheritdoc />
    public abstract IParseResult<DateTime?> EvaluateDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}