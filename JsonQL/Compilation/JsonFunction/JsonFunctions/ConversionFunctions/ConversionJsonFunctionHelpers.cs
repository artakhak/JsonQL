using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

public static class ConversionJsonFunctionHelpers
{
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