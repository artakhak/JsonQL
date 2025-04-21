using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

public static class AssertOperatorFunctionHelpers
{
    public static IParseResult<T?> GetParseResultWithErrorIfValueIsNull<T>(IParseResult<T?> parseResult, IJsonLineInfo? lineInfo)
    {
        if (parseResult.Errors.Count > 0)
            return parseResult;

        if (parseResult.Value == null || parseResult.Value is IJsonValuePathLookupResult {HasValue: false})
            return new ParseResult<T>(CollectionExpressionHelpers.Create(new JsonObjectParseError("Value not-null assertion failed", lineInfo)));
        
        return parseResult;
    }
}