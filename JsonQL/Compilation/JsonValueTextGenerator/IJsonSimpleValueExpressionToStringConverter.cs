// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueTextGenerator;

/// <summary>
/// Converts the result of <see cref="IJsonFunction.EvaluateValue"/>() to <see cref="string"/> value.
/// </summary>
public interface IJsonSimpleValueExpressionToStringConverter
{
    IParseResult<string> GenerateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
}

/// <summary>
/// Implements the conversion of evaluated JSON simple value expressions to string values.
/// </summary>
public class JsonSimpleValueExpressionToStringConverter : IJsonSimpleValueExpressionToStringConverter
{
    private readonly IStringFormatter _stringFormatter;
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Implements the conversion of evaluated JSON simple value expressions to their string representations.
    /// Utilizes a string formatter and JSON function to process and format the values.
    /// </summary>
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

        if (parsedValue == null)
            return new ParseResult<string>(string.Empty);

        if (!_stringFormatter.TryFormat(parsedValue, out var formattedValue))
            formattedValue = String.Empty;

        return new ParseResult<string>(formattedValue);
    }
}
