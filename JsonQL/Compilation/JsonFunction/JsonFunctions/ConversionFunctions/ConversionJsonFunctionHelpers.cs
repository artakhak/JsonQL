// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

/// <summary>
/// Provides utility methods to assist with JSON function conversions, specifically handling parsing results and errors
/// encountered during type conversions within the context of JSON functions.
/// </summary>
public static class ConversionJsonFunctionHelpers
{
    /// <summary>
    /// Creates a parse result to handle conversion errors based on the provided parameters.
    /// </summary>
    /// <typeparam name="T">The type to which the value is being converted.</typeparam>
    /// <param name="convertedTypeName">The name of the type to which the conversion is attempted.</param>
    /// <param name="assertIfConversionFailsJsonFunction">The JSON function representing the condition to assert if the conversion fails.</param>
    /// <param name="assertIfConversionFailsResult">The result of evaluating the condition for asserting conversion failure.</param>
    /// <param name="lineInfo">Optional line information for error reporting.</param>
    /// <returns>
    /// A parse result containing the errors related to conversion, or null if no errors occurred.
    /// </returns>
    public static IParseResult<T?>? GetParseResultForConversionError<T>(string convertedTypeName,
        IBooleanJsonFunction assertIfConversionFailsJsonFunction,
        IParseResult<bool?> assertIfConversionFailsResult, IJsonLineInfo? lineInfo)
    {
        if (assertIfConversionFailsResult.Errors.Count > 0)
            return new ParseResult<T?>(assertIfConversionFailsResult.Errors);

        if (assertIfConversionFailsResult.Value == null)
        {
            return new ParseResult<T?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("The value of 'assertIfConversionFails' parameter is not null and failed to parse.", 
                assertIfConversionFailsJsonFunction.LineInfo)));
        }

        if (!assertIfConversionFailsResult.Value.Value)
            return null;

        return new ParseResult<T>(CollectionExpressionHelpers.Create(new JsonObjectParseError($"Failed to convert the value to [{convertedTypeName}].", lineInfo)));
    }
}