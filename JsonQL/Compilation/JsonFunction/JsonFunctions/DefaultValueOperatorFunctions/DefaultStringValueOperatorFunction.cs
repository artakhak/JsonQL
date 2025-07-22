// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

/// <summary>
/// Represents a function to handle default string value operations within a JSON context.
/// This specialized implementation operates specifically for string-based JSON functions,
/// enabling evaluation and manipulation of primary and default string values.
/// </summary>
/// <remarks>
/// This class extends <see cref="DefaultValueOperatorFunction"/> and implements <see cref="IStringJsonFunction"/>
/// to provide functionality for string-specific default value operations.
/// </remarks>
public class DefaultStringValueOperatorFunction : DefaultValueOperatorFunction, IStringJsonFunction
{
    /// <summary>
    /// Represents a specialized implementation of the default value operator function that operates on string-based JSON values.
    /// This class combines a main value JSON function and a default value JSON function, ensuring that a valid string result is produced by falling back to the default value when necessary.
    /// </summary>
    /// <param name="operatorName">The name of the operator associated with this function.</param>
    /// <param name="mainValueJsonFunction">The main evaluation function used to retrieve the primary value.</param>
    /// <param name="defaultValueJsonFunction">The fallback evaluation function used to retrieve the default value if the main function fails.</param>
    /// <param name="jsonFunctionContext">The context object providing evaluation information for this function's execution.</param>
    /// <param name="lineInfo">Optional line information for error reporting or diagnostics.</param>
    /// <remarks>
    /// Inherits from the <see cref="DefaultValueOperatorFunction"/> class, adding functionality specific to string results.
    /// Implements the <see cref="IStringJsonFunction"/> interface to indicate compatibility with string-based JSON data.
    /// </remarks>
    public DefaultStringValueOperatorFunction(string operatorName, IStringJsonFunction mainValueJsonFunction, IStringJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToString(LineInfo);
    }
}
