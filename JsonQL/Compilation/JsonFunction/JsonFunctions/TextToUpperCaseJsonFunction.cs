// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function for transforming text to uppercase.
/// </summary>
/// <remarks>
/// This class is an implementation of a text transformation function within the JSONQL framework.
/// It performs a transformation to convert the provided string value to uppercase.
/// </remarks>
public class TextToUpperCaseJsonFunction : TextTransformationJsonFunctionAbstr
{
    /// <summary>
    /// Defines a JSON function that applies a transformation to convert text to upper case
    /// within a JSON processing context.
    /// </summary>
    /// <param name="functionName">Function name.</param>
    /// <param name="stringJsonFunction">The JSON function to be transformed.</param>
    /// <param name="jsonFunctionContext">The context for JSON function value evaluation.</param>
    /// <param name="lineInfo">Optional line information for error handling and debugging.</param>
    public TextToUpperCaseJsonFunction(string functionName, IJsonFunction stringJsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, stringJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<string> ConvertString(string value)
    {
        return new ParseResult<string>(value.ToUpper());
    }
}
