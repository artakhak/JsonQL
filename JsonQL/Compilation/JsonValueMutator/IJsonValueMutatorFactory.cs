// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Text;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonValueMutator.JsonValueMutators;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Factory interface for creating instances of <see cref="IJsonValueMutator"/> based on specified parameters.
/// </summary>
public interface IJsonValueMutatorFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IJsonValueMutator"/> using the specified parameters.
    /// </summary>
    /// <param name="jsonObjectData">The JSON object data to be used for creating the mutator.</param>
    /// <param name="parsedSimpleValue">The parsed simple value to be used for initialization.</param>
    /// <param name="parsedExpressionsData">A collection of parsed expressions data to be used in the mutator creation process.</param>
    /// <returns>A result containing the created <see cref="IJsonValueMutator"/> or a collection of errors if creation fails.</returns>
    IParseResult<IJsonValueMutator> Create(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue,
        IReadOnlyList<IParsedExpressionData> parsedExpressionsData);
}

/// <inheritdoc />
public class JsonValueMutatorFactory : IJsonValueMutatorFactory
{
    private readonly IVariablesManagerFactory _variablesManagerFactory;
    private readonly IJsonValuePathJsonFunctionParser _jsonValuePathJsonFunctionParser;
    private readonly IJsonFunctionFromExpressionParser _jsonFunctionFromExpressionParser;
    private readonly ICalculatedValueJsonValueMutatorFactory _calculatedValueJsonValueMutatorFactory;
    private readonly IMergeCollectionIntoArrayJsonValueMutatorFactory _mergeCollectionIntoArrayJsonValueMutatorFactory;
    private readonly ICopyFieldsJsonValueMutatorFactory _copyFieldsJsonValueMutatorFactory;
    private readonly IJsonSimpleValueMutatorFactory _jsonSimpleValueMutatorFactory;
    private readonly IJsonValueTextGeneratorFactory _jsonValueTextGeneratorFactory;

    private readonly HashSet<string> _copyObjectFunctionNames = new(StringComparer.Ordinal)
    {
        {JsonMutatorKeywords.MergedArrayItems},
        {JsonMutatorKeywords.MergedJsonObjectFields},
        {JsonMutatorKeywords.CalculatedJsonObjectValue}
    };

    public JsonValueMutatorFactory(
        IVariablesManagerFactory variablesManagerFactory,
        IJsonValuePathJsonFunctionParser jsonValuePathJsonFunctionParser,
        IJsonFunctionFromExpressionParser jsonFunctionFromExpressionParser,
        ICalculatedValueJsonValueMutatorFactory calculatedValueJsonValueMutatorFactory,
        IMergeCollectionIntoArrayJsonValueMutatorFactory mergeCollectionIntoArrayJsonValueMutatorFactory,
        ICopyFieldsJsonValueMutatorFactory copyFieldsJsonValueMutatorFactory,
        IJsonSimpleValueMutatorFactory jsonSimpleValueMutatorFactory,
        IJsonValueTextGeneratorFactory jsonValueTextGeneratorFactory)
    {
        _variablesManagerFactory = variablesManagerFactory;
        _jsonValuePathJsonFunctionParser = jsonValuePathJsonFunctionParser;
        _jsonFunctionFromExpressionParser = jsonFunctionFromExpressionParser;
        _calculatedValueJsonValueMutatorFactory = calculatedValueJsonValueMutatorFactory;
        _mergeCollectionIntoArrayJsonValueMutatorFactory = mergeCollectionIntoArrayJsonValueMutatorFactory;
        _copyFieldsJsonValueMutatorFactory = copyFieldsJsonValueMutatorFactory;
        _jsonSimpleValueMutatorFactory = jsonSimpleValueMutatorFactory;
        _jsonValueTextGeneratorFactory = jsonValueTextGeneratorFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValueMutator> Create(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue,
        IReadOnlyList<IParsedExpressionData> parsedExpressionsData)
    {
        if (parsedExpressionsData.Count == 0)
            return new ParseResult<IJsonValueMutator>(
                    CollectionExpressionHelpers.Create(
                    new JsonObjectParseError($"The list [{nameof(parsedExpressionsData)}] is empty in [{nameof(JsonValueMutatorFactory)}.{nameof(Create)}]", parsedSimpleValue.LineInfo)
                ));

        var parsedText = parsedSimpleValue.Value;
        if (parsedText == null)
            return new ParseResult<IJsonValueMutator>(
                    CollectionExpressionHelpers.Create(
                new JsonObjectParseError($"The json value is null in [{nameof(JsonValueMutatorFactory)}.{nameof(Create)}]", parsedSimpleValue.LineInfo)
            ));

        var expressionData = parsedExpressionsData.FirstOrDefault(
                parsedExpressionData =>
                    _copyObjectFunctionNames.Contains(parsedExpressionData.MutatorFunctionName));

        if (expressionData != null)
        {
            if (parsedExpressionsData.Count > 1)
            {
                return new ParseResult<IJsonValueMutator>(
                        CollectionExpressionHelpers.Create(
                    new JsonObjectParseError($"Special function {expressionData.MutatorFunctionName} cannot be used in json value that has other expressions that start with '$(' or '$...('",
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionData.TemplateStartIndex))
                ));
            }

            if (expressionData.TemplateStartIndex + expressionData.TemplateLength != parsedText.Length)
            {
                return new ParseResult<IJsonValueMutator>(
                        CollectionExpressionHelpers.Create(
                    new JsonObjectParseError($"Special function {expressionData.MutatorFunctionName}(...) cannot be followed by any characters",
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionData.TemplateStartIndex))
                ));
            }

            return TryParseCopyObjectsJsonValueMutator(parsedSimpleValue, expressionData);
        }

        return TryParseSimpleValueMutator(parsedSimpleValue, parsedText, parsedExpressionsData);
    }

    private ParseResult<IJsonValueMutator> TryParseCopyObjectsJsonValueMutator(IParsedSimpleValue parsedSimpleValue,
        IParsedExpressionData copyObjectsFunctionData)
    {
        switch (copyObjectsFunctionData.MutatorFunctionName)
        {
            case JsonMutatorKeywords.MergedJsonObjectFields:
                {
                    var parsePathErrors = new List<IJsonObjectParseError>();
                    var jsonValuePathJsonFunction = TryParseJsonValuePathJsonFunction(parsedSimpleValue,
                        copyObjectsFunctionData.ParsedExpressionItem, parsePathErrors);

                    if (parsePathErrors.Count > 0 || jsonValuePathJsonFunction == null)
                        return new ParseResult<IJsonValueMutator>(parsePathErrors);

                    return new ParseResult<IJsonValueMutator>(_copyFieldsJsonValueMutatorFactory.Create(parsedSimpleValue, jsonValuePathJsonFunction));
                }

            case JsonMutatorKeywords.MergedArrayItems:
                {
                    var parsePathErrors = new List<IJsonObjectParseError>();
                    var jsonFunction = TryParseJsonFunction(parsedSimpleValue,
                        copyObjectsFunctionData.ParsedExpressionItem, parsePathErrors);

                    if (parsePathErrors.Count > 0 || jsonFunction == null)
                        return new ParseResult<IJsonValueMutator>(parsePathErrors);

                    return new ParseResult<IJsonValueMutator>(_mergeCollectionIntoArrayJsonValueMutatorFactory.Create(parsedSimpleValue, jsonFunction));
                }

            case JsonMutatorKeywords.CalculatedJsonObjectValue:
                {
                    var parsePathErrors = new List<IJsonObjectParseError>();
                    var jsonFunction = TryParseJsonFunction(parsedSimpleValue,
                        copyObjectsFunctionData.ParsedExpressionItem, parsePathErrors);

                    if (parsePathErrors.Count > 0 || jsonFunction == null)
                        return new ParseResult<IJsonValueMutator>(parsePathErrors);

                    return new ParseResult<IJsonValueMutator>(_calculatedValueJsonValueMutatorFactory.Create(parsedSimpleValue, jsonFunction));
                }
        }

        return new ParseResult<IJsonValueMutator>(
                CollectionExpressionHelpers.Create(
            new JsonObjectParseError($"Cannot parse expression ['{parsedSimpleValue.Value}'] into a special expression.",
                parsedSimpleValue.LineInfo.GenerateRelativePosition(copyObjectsFunctionData.TemplateStartIndex))
        ));
    }

    private IParseResult<IJsonValueMutator> TryParseSimpleValueMutator(IParsedSimpleValue parsedSimpleValue,
        string parsedText,
        IReadOnlyList<IParsedExpressionData> parsedExpressionsData)
    {
        var parsedTextGeneratorExpressions = new List<IJsonSimpleValueExpressionToStringConverter>(parsedExpressionsData.Count);
        var expressionsTemplateTextStrBldr = new StringBuilder(parsedText.Length);

        var currentIndexInValue = 0;

        var currentTemplateIndex = 0;
        var parsedExpressionData = parsedExpressionsData[currentTemplateIndex];
        while (currentIndexInValue < parsedText.Length)
        {
            if (currentIndexInValue == parsedExpressionData.TemplateStartIndex)
            {
                var parseResult =
                    _jsonValueTextGeneratorFactory.Create(parsedSimpleValue, _variablesManagerFactory.Create(), parsedExpressionData.ParsedExpressionItem);

                if (parseResult.Errors.Count > 0 || parseResult.Value == null)
                {
                    if (parseResult.Errors.Count == 0)
                    {
                        return new ParseResult<IJsonValueMutator>(
                                CollectionExpressionHelpers.Create(
                            new JsonObjectParseError("Failed to parse expression",
                                parsedSimpleValue.LineInfo.GenerateRelativePosition(parsedExpressionData.TemplateStartIndex))
                        ));
                    }

                    return new ParseResult<IJsonValueMutator>(parseResult.Errors);
                }

                expressionsTemplateTextStrBldr.Append('{');
                expressionsTemplateTextStrBldr.Append(currentTemplateIndex);
                expressionsTemplateTextStrBldr.Append('}');

                parsedTextGeneratorExpressions.Add(parseResult.Value);

                currentIndexInValue = parsedExpressionData.TemplateStartIndex + parsedExpressionData.TemplateLength;

                ++currentTemplateIndex;

                if (currentTemplateIndex < parsedExpressionsData.Count)
                    parsedExpressionData = parsedExpressionsData[currentTemplateIndex];
            }
            else
            {
                expressionsTemplateTextStrBldr.Append(parsedText[currentIndexInValue]);
                ++currentIndexInValue;
            }
        }

        return new ParseResult<IJsonValueMutator>(
            _jsonSimpleValueMutatorFactory.Create(parsedSimpleValue, parsedTextGeneratorExpressions, expressionsTemplateTextStrBldr.ToString()));
    }

    private IJsonValuePathJsonFunction? TryParseJsonValuePathJsonFunction(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase expressionItem,
        List<IJsonObjectParseError> errors)
    {
        var context = new JsonFunctionValueEvaluationContext(_variablesManagerFactory.Create());
        var jsonPathParseResult = _jsonValuePathJsonFunctionParser.TryParse(parsedSimpleValue, expressionItem, context);

        if (jsonPathParseResult.Errors.Count > 0)
        {
            errors.AddRange(jsonPathParseResult.Errors);
            return null;
        }

        if (jsonPathParseResult.Value == null)
        {
            errors.Add(new JsonObjectParseError("Failed to parse json path.", 
                parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionItem.IndexInText)));
            return null;
        }

        return jsonPathParseResult.Value;
    }

    private IJsonFunction? TryParseJsonFunction(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase expressionItem,
        List<IJsonObjectParseError> errors)
    {
        var context = new JsonFunctionValueEvaluationContext(_variablesManagerFactory.Create());

        var jsonParseResult = _jsonFunctionFromExpressionParser.Parse(parsedSimpleValue, expressionItem, context);

        if (jsonParseResult.Errors.Count > 0)
        {
            errors.AddRange(jsonParseResult.Errors);
            return null;
        }

        if (jsonParseResult.Value == null)
        {
            errors.Add(new JsonObjectParseError("Failed to parse json function from expression.", 
                parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionItem.IndexInText)));
            return null;
        }

        return jsonParseResult.Value;
    }
}