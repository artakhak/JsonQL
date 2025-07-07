// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an abstract base class for JSON functions that operate on string values.
/// This class serves as a foundation for implementing specific JSON functions handling
/// string-related operations.
/// </summary>
public abstract class StringJsonFunctionAbstr : JsonFunctionAbstr, IStringJsonFunction
{
    /// <summary>
    /// Represents an abstract base class for JSON functions that operate on strings.
    /// Inherits from <see cref="JsonFunctionAbstr"/> and implements <see cref="IStringJsonFunction"/>.
    /// Serves as a foundation for implementing JSON functions that handle operations involving string values.
    /// </summary>
    protected StringJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateStringValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <summary>
    /// Retrieves a string value based on the provided parsed root value, parent parsed values, and context data.
    /// This method serves as the core implementation for extracting or deriving string data from JSON-based inputs.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value representing the primary input for the JSON structure.</param>
    /// <param name="compiledParentRootParsedValues">A collection of parent parsed values that contribute context
    /// or additional data to the evaluation process.</param>
    /// <param name="contextData">Optional context-specific data that can inform or modify the processing logic.</param>
    /// <returns>A parsed result containing the string value derived from the input parameters, or null if
    /// the operation has no corresponding output.</returns>
    public abstract IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}