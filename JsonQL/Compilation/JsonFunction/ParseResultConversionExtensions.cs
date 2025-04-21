using OROptimizer.Diagnostics.Log;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

public static class ParseResultConversionExtensions
{
    public static IParseResult<object?> ConvertToObject<TValue>(this IParseResult<TValue?> parseResult)
    {
        if (parseResult.Errors.Count > 0)
            return new ParseResult<object?>(parseResult.Errors);

        if (parseResult.Value == null)
            return new ParseResult<object?>((object?) null);

        return new ParseResult<object?>(parseResult.Value);
    }

    public static IParseResult<double?> ConvertToDouble(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo)
    {
        return parseResult.ConvertToStruct<double>(jsonLineInfo);
    }

    public static IParseResult<bool?> ConvertToBoolean(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo)
    {
        return parseResult.ConvertToStruct<bool>(jsonLineInfo);
    }

    public static IParseResult<DateTime?> ConvertToDateTime(this IParseResult<object?> parseResult, IJsonLineInfo? jsonLineInfo)
    {
        return parseResult.ConvertToStruct<DateTime>(jsonLineInfo);
    }

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
            LogHelper.Context.Log.DebugFormat("Failed to convert [{0}] to [{1}].",
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
            LogHelper.Context.Log.DebugFormat("Failed to convert [{0}] to [{1}].",
                parseResult.Value, typeof(TValue));
            return new ParseResult<TValue?>((TValue?)null);
        }

        return new ParseResult<TValue?>(comparableValue);
    }
}