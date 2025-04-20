using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

public interface IJsonValuePathLookupResult
{
    bool IsSingleItemLookup { get; }
    bool HasValue { get; }
    //IParsedSimpleValueAttachedValues ParsedSimpleValueAttachedValues { get; }
}

public static class JsonValuePathLookupResultExtensions
{
    public static IParseResult<IReadOnlyList<IParsedValue>> GetResultAsParsedValuesList(this IJsonValuePathLookupResult jsonValuePathLookupResult, 
        bool flattenSingleValueArrayResult,
        IJsonLineInfo? jsonLineInfo)
    {
        if (jsonValuePathLookupResult is ICollectionJsonValuePathLookupResult collectionJsonValuePathLookupResult)
            return new ParseResult<IReadOnlyList<IParsedValue>>(collectionJsonValuePathLookupResult.ParsedValues);

        if (jsonValuePathLookupResult is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult)
        {
            if (singleItemJsonValuePathLookupResult.ParsedValue == null)
                return new ParseResult<IReadOnlyList<IParsedValue>>(Array.Empty<IParsedValue>());

            if (flattenSingleValueArrayResult && singleItemJsonValuePathLookupResult.ParsedValue is IParsedArrayValue parsedArrayValue)
                return new ParseResult<IReadOnlyList<IParsedValue>>(parsedArrayValue.Values);

            return new ParseResult<IReadOnlyList<IParsedValue>>(new List<IParsedValue>
            {
                singleItemJsonValuePathLookupResult.ParsedValue
            });
        }

        return new ParseResult<IReadOnlyList<IParsedValue>>(CollectionExpressionHelpers.Create(
            new JsonObjectParseError(
                $"Invalid result type [{jsonValuePathLookupResult.GetType()}]", jsonLineInfo)
        ));
    }
}