using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonFunction;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueTextGenerator;

/// <summary>
/// Converts the result of <see cref="IJsonFunction.EvaluateValue"/>() to <see cref="string"/> value.
/// </summary>
public interface IJsonSimpleValueExpressionToStringConverter
{
    IParseResult<string> GenerateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
}

public class JsonSimpleValueExpressionToStringConverter : IJsonSimpleValueExpressionToStringConverter
{
    private readonly IStringFormatter _stringFormatter;
    private readonly IJsonFunction _jsonFunction;

    public JsonSimpleValueExpressionToStringConverter(IStringFormatter stringFormatter, IJsonFunction jsonFunction)
    {
        _stringFormatter = stringFormatter;
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public IParseResult<string> GenerateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        var jsonFunctionParseResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, null);

        if (jsonFunctionParseResult.Errors.Count > 0)
            return new ParseResult<string>(jsonFunctionParseResult.Errors);

        var parsedValue = jsonFunctionParseResult.Value;

        if (parsedValue is IJsonValuePathLookupResult)
        {
            IParsedSimpleValue? parsedSimpleValue;

            if (parsedValue is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult &&
                (parsedSimpleValue = singleItemJsonValuePathLookupResult.ParsedValue as IParsedSimpleValue) != null)
            {
                parsedValue = parsedSimpleValue.Value;
            }
            else if (parsedValue is ICollectionJsonValuePathLookupResult { ParsedValues.Count: 1 } collectionJsonValuePathLookupResult &&
                     (parsedSimpleValue = collectionJsonValuePathLookupResult.ParsedValues[0] as IParsedSimpleValue) != null)
            {
                parsedValue = parsedSimpleValue.Value;
            }
            else
                parsedValue = string.Empty;
        }

        //if (jsonFunctionParseResult.Value is IEnumerable<IParsedValue> parsedValues)
        //{
        //    var firstParsedValue = parsedValues.FirstOrDefault();
        //    if (firstParsedValue is IParsedSimpleValue parsedSimpleValue)
        //    {
        //        return new JsonValueTextGeneratorResult(parsedSimpleValue.Value ?? string.Empty);
        //    }

        //    return new JsonValueTextGeneratorResult(string.Empty);
        //}

        if (parsedValue == null)
            return new ParseResult<string>(string.Empty);

        if (!_stringFormatter.TryFormat(parsedValue, out var formattedValue))
            formattedValue = String.Empty;

        return new ParseResult<string>(formattedValue);
    }
}