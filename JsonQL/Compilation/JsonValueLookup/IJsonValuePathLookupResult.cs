using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

/// <summary>
/// Defines the result of a JSON value path lookup operation. This result can be used to determine<br/>
/// if the lookup was successful and whether the resulting value represents a single item or<br/>
/// a collection of items.<br/>
/// The details of lookup are in sub-interfaces <see cref="ICollectionJsonValuePathLookupResult"/> ane <see cref="ICollectionJsonValuePathLookupResult"/>.
/// </summary>
public interface IJsonValuePathLookupResult
{
    /// <summary>
    /// Indicates whether the result of the JSON value path lookup operation represents
    /// a single item.
    /// </summary>
    /// <value>
    /// Returns <c>true</c> if the lookup result corresponds to a single item; otherwise, <c>false</c>.
    /// </value>
    bool IsSingleItemLookup { get; }

    /// <summary>
    /// Indicates whether the result of the JSON value path lookup operation contains
    /// any value.
    /// </summary>
    /// <value>
    /// Returns <c>true</c> if the lookup result contains one or more values; otherwise, <c>false</c>.
    /// </value>
    bool HasValue { get; }
    //IParsedSimpleValueAttachedValues ParsedSimpleValueAttachedValues { get; }
}

/// <summary>
/// Provides extension methods for working with instances of <see cref="IJsonValuePathLookupResult"/>.
/// These extensions allow the retrieval and processing of result values from JSON value path lookup operations.
/// </summary>
public static class JsonValuePathLookupResultExtensions
{
    /// <summary>
    /// Converts the result of a JSON value path lookup into a list of parsed values.
    /// </summary>
    /// <param name="jsonValuePathLookupResult">
    /// An implementation of <see cref="IJsonValuePathLookupResult"/> representing the lookup result from a JSON value path query.
    /// </param>
    /// <param name="flattenSingleValueArrayResult">
    /// A boolean value indicating whether single-value arrays should be flattened into their individual values.<br/>
    /// For example, if this is value is true and the result is <see cref="IParsedArrayValue"/>,<br/>
    /// the returned value will have a collection of <see cref="IParsedValue"/> generated from <see cref="IParsedArrayValue.Values"/><br/>
    /// instead of the original <see cref="IParsedArrayValue"/>.
    /// </param>
    /// <param name="jsonLineInfo">
    /// Optional line information (<see cref="IJsonLineInfo"/>) to include in case of errors during parsing.
    /// </param>
    /// <returns>
    /// An instance of <see cref="IParseResult{TValue}"/> containing a read-only list of parsed values (<see cref="IParsedValue"/>),
    /// or errors if the lookup result cannot be converted.
    /// </returns>
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