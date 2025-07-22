// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Provides a set of extension methods for converting instances of <see cref="IParseResult{TValue}"/>
/// to other types, facilitating seamless transformations while maintaining parsing errors and information.
/// </summary>
public static class ParseResultConversionExtensions
{
    /// <summary>
    /// Converts the value of the provided parse result to an object type while preserving any associated errors.
    /// </summary>
    /// <param name="parseResult">The parse result containing the value and any associated errors.</param>
    /// <typeparam name="TValue">The original type of the value in the parse result.</typeparam>
    /// <returns>An <see cref="IParseResult{T}"/> with the value converted to an object type, or any errors carried over from the input parse result.</returns>
    public static IParseResult<object?> ConvertToObject<TValue>(this IParseResult<TValue?> parseResult)
    {
        if (parseResult.Errors.Count > 0)
            return new ParseResult<object?>(parseResult.Errors);

        if (parseResult.Value == null)
            return new ParseResult<object?>((object?) null);

        return new ParseResult<object?>(parseResult.Value);
    }

    /// <summary>
    /// Converts the value of the provided parse result to a nullable double while preserving any associated errors.
    /// </summary>
    /// <param name="parseResult">The parse result containing the value to be converted and any associated errors.</param>
    /// <param name="jsonLineInfo">Optional line information for the source of the parse result, which may assist with error reporting.</param>
    /// <returns>An <see cref="IParseResult{T}"/> containing the converted nullable double value, or any errors carried over from the input parse result.</returns>
    public static IParseResult<double?> ConvertToDouble(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo)
    {
        return parseResult.ConvertToStruct<double>(jsonLineInfo);
    }

    /// <summary>
    /// Converts the value of the provided parse result to a nullable boolean type while preserving any associated errors.
    /// </summary>
    /// <param name="parseResult">The parse result containing the value to be converted and any associated errors.</param>
    /// <param name="jsonLineInfo">Optional JSON line information to be associated with the converted result.</param>
    /// <returns>An <see cref="IParseResult{T}"/> with the value converted to a nullable boolean, or any errors carried over from the input parse result.</returns>
    public static IParseResult<bool?> ConvertToBoolean(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo)
    {
        return parseResult.ConvertToStruct<bool>(jsonLineInfo);
    }

    /// <summary>
    /// Converts the value of the provided parse result to a <see cref="DateTime"/> type, preserving any associated errors.
    /// </summary>
    /// <param name="parseResult">The parse result containing the value to convert and any associated errors.</param>
    /// <param name="jsonLineInfo">Optional line information used for error reporting during the conversion.</param>
    /// <returns>An <see cref="IParseResult{DateTime}"/> containing the converted value or any errors from the input parse result.</returns>
    public static IParseResult<DateTime?> ConvertToDateTime(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo)
    {
        return parseResult.ConvertToStruct<DateTime>(jsonLineInfo);
    }

    /// <summary>
    /// Converts the value of the provided parse result to a string type while preserving any associated errors.
    /// </summary>
    /// <param name="parseResult">The parse result containing the original value and parsing errors, if any.</param>
    /// <param name="jsonLineInfo">Optional object providing line information for error tracking in JSON parsing.</param>
    /// <returns>An <see cref="IParseResult{T}"/> with the value converted to a string, or any errors carried over from the input parse result.</returns>
    public static IParseResult<string?> ConvertToString(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo)
    {
        return parseResult.ConvertToReferenceTypeValue<string>(jsonLineInfo);
    }
    
    private static IParseResult<TValue?> ConvertToStruct<TValue>(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo) where TValue: struct
    {
        if (parseResult.Errors.Count > 0)
            return new ParseResult<TValue?>(parseResult.Errors);
       
        if (parseResult.Value == null)
            return new ParseResult<TValue?>((TValue?)null);

        if (parseResult.Value is not TValue comparableValue)
        {
            ThreadStaticLoggingContext.Context.DebugFormat("Failed to convert [{0}] to [{1}].",
                parseResult.Value, typeof(TValue));
            return new ParseResult<TValue?>((TValue?)null);
        }

        return new ParseResult<TValue?>(comparableValue);
    }

    private static IParseResult<TValue?> ConvertToReferenceTypeValue<TValue>(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo) where TValue : class
    {
        if (parseResult.Errors.Count > 0)
            return new ParseResult<TValue?>(parseResult.Errors);

        if (parseResult.Value == null)
            return new ParseResult<TValue?>((TValue?)null);

        if (parseResult.Value is not TValue comparableValue)
        {
            ThreadStaticLoggingContext.Context.DebugFormat("Failed to convert [{0}] to [{1}].",
                parseResult.Value, typeof(TValue));
            return new ParseResult<TValue?>((TValue?)null);
        }

        return new ParseResult<TValue?>(comparableValue);
    }
}
