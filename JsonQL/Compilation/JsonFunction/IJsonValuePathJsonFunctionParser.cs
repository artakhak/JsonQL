// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Defines the contract for parsing JSON path expressions and creating corresponding JSON path functions.
/// </summary>
public interface IJsonValuePathJsonFunctionParser
{
    /// <summary>
    /// Tries to parse and create an instance of <see cref="IJsonValuePathJsonFunction"/> using the provided parameters.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value used to extract JSON path data.</param>
    /// <param name="expressionItem">The expression item representing either an operator or a literal expression item.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function, providing additional information for interpretation.</param>
    /// <returns>An <see cref="IParseResult{T}"/> containing the parsed <see cref="IJsonValuePathJsonFunction"/>, or null if parsing fails.</returns>
    IParseResult<IJsonValuePathJsonFunction?> TryParse(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase expressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

/// <inheritdoc />
public class JsonValuePathJsonFunctionParser : IJsonValuePathJsonFunctionParser
{
    private readonly IJsonValuePathLookup _jsonValuePathLookup;

    private readonly IJsonValueCollectionItemsSelectorPathElementFactory _collectionItemsSelectorPathElementFactory;

    /// <summary>
    /// Represents a parser responsible for creating instances of <see cref="IJsonValuePathJsonFunction"/>
    /// by analyzing the provided JSON path elements and resolving them into a functional structure.
    /// </summary>
    public JsonValuePathJsonFunctionParser(IJsonValuePathLookup jsonValuePathLookup,
        IJsonValueCollectionItemsSelectorPathElementFactory collectionItemsSelectorPathElementFactory)
    {
        _jsonValuePathLookup = jsonValuePathLookup;
        _collectionItemsSelectorPathElementFactory = collectionItemsSelectorPathElementFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathJsonFunction?> TryParse(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase expressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        // Add key function. Example is Object1.key(1).Array1[1]
        if (!(expressionItem is ILiteralExpressionItem ||
              expressionItem is IBracesExpressionItem bracesExpressionItem &&
              (!bracesExpressionItem.OpeningBrace.IsRoundBrace ||
               bracesExpressionItem.NameLiteral != null &&
               _collectionItemsSelectorPathElementFactory.IsJsonValueCollectionItemsSelectorFunction(bracesExpressionItem.NameLiteral.LiteralName.Text) ) ||
              expressionItem is IOperatorExpressionItem 
                  {OperatorInfoExpressionItem.OperatorInfo: {OperatorType: OperatorType.BinaryOperator, Name: JsonOperatorNames.JsonPathSeparator}}))
            return new ParseResult<IJsonValuePathJsonFunction?>((IJsonValuePathJsonFunction?)null);

        var jsonPathElements = new List<IJsonValuePathElement>();
        var parseErrors = new List<IJsonObjectParseError>();
        
        Parse(parsedSimpleValue, expressionItem, jsonPathElements, jsonFunctionContext, parseErrors);

        if (parseErrors.Count > 0 || jsonPathElements.Count == 0)
        {
            if (parseErrors.Count == 0)
            {
                parseErrors.Add(new JsonObjectParseError("Failed to parse Json path.", parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionItem)));
            }

            return new ParseResult<IJsonValuePathJsonFunction>(parseErrors);
        }

        string GetJsonValuePathFunctionName(JsonValuePath jsonValuePath)
        {
            var jsonValuePathToString = jsonValuePath.ToString();

            var maxNameLength = 30;
            return $"Path:{(jsonValuePathToString.Length < maxNameLength ? jsonValuePathToString : jsonValuePathToString.Substring(0, maxNameLength))}";
        }

        var jsonValuePath = new JsonValuePath(jsonPathElements);
        
        return new ParseResult<IJsonValuePathJsonFunction?>(new JsonValuePathJsonFunction(GetJsonValuePathFunctionName(jsonValuePath), jsonValuePath, _jsonValuePathLookup,
            jsonFunctionContext, jsonPathElements[0].LineInfo));
    }

    private bool Parse(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase jsonPathExpressionItem, List<IJsonValuePathElement> jsonPathElements,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        List<IJsonObjectParseError> parseErrors)
    {
        var parsedJsonPathElements = TryParseJsonPathElement(parsedSimpleValue, jsonPathExpressionItem,
            jsonPathElements, jsonFunctionContext, parseErrors);

        if (parseErrors.Count > 0)
        {
            return false;
        }

        if (parsedJsonPathElements != null)
        {
            if (parsedJsonPathElements.Count == 0)
            {
                parseErrors.Add(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(jsonPathExpressionItem)));
                return false;
            }

            jsonPathElements.AddRange(parsedJsonPathElements);
            return true;
        }

        if (jsonPathExpressionItem is IOperatorExpressionItem operatorExpressionItem)
        {
            if (operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.OperatorType != OperatorType.BinaryOperator ||
                operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name != JsonOperatorNames.JsonPathSeparator)
            {
                parseErrors.Add(new JsonObjectParseError($"Invalid symbol. Expected '[{JsonOperatorNames.JsonPathSeparator}]'.",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(operatorExpressionItem.OperatorInfoExpressionItem)));
                return false;
            }

            if (operatorExpressionItem.Operand2 == null)
            {
                parseErrors.Add(new JsonObjectParseError($"Invalid symbol. Path element is missing after '[{JsonOperatorNames.JsonPathSeparator}]'.",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(operatorExpressionItem.OperatorInfoExpressionItem)));
                return false;
            }

            if (Parse(parsedSimpleValue, operatorExpressionItem.Operand1, jsonPathElements, jsonFunctionContext, parseErrors))
            {
                if (Parse(parsedSimpleValue, operatorExpressionItem.Operand2, jsonPathElements, jsonFunctionContext, parseErrors))
                {
                    return true;
                }

                if (parseErrors.Count > 0)
                    return false;
            }
            else
            {
                if (parseErrors.Count > 0)
                    return false;
            }
        }

        parseErrors.Add(new JsonObjectParseError("Invalid symbol", parsedSimpleValue.LineInfo.GenerateRelativePosition(jsonPathExpressionItem)));
        return false;
    }

    private IReadOnlyList<IJsonValuePathElement>? TryParseJsonPathElement(IParsedSimpleValue parsedSimpleValue,
        IExpressionItemBase expressionItem,
        IReadOnlyList<IJsonValuePathElement> currentJsonPathElements,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        List<IJsonObjectParseError> parseErrors)
    {
        if (expressionItem is ILiteralExpressionItem literalExpressionItem)
        {
            JsonValuePropertyNamePathElement jsonValuePropertyNamePathElement = new JsonValuePropertyNamePathElement(literalExpressionItem.LiteralName.Text,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(literalExpressionItem));

            if (!ValidateIsNotPrecededWithCollectionSelector(currentJsonPathElements, jsonValuePropertyNamePathElement, parseErrors,
                    "json field name"))
                return null;
            
            return CollectionExpressionHelpers.Create(jsonValuePropertyNamePathElement);
        }

        if (expressionItem is IBracesExpressionItem bracesExpressionItem)
        {
            if (bracesExpressionItem.OpeningBrace.IsRoundBrace)
            {
                // Try parse items selectors, such as 'Where', 'First', etc.
                if (bracesExpressionItem.NameLiteral == null)
                {
                    parseErrors.Add(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem)));
                    return null;
                }
                
                var collectionItemsSelectorPathElementResult = _collectionItemsSelectorPathElementFactory.Create(
                    parsedSimpleValue, bracesExpressionItem, bracesExpressionItem.NameLiteral.LiteralName.Text,
                    jsonFunctionContext,
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem.NameLiteral));

                if (collectionItemsSelectorPathElementResult.Errors.Count > 0 || collectionItemsSelectorPathElementResult.Value == null)
                {
                    if (collectionItemsSelectorPathElementResult.Errors.Count == 0)
                        parseErrors.Add(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                            parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem.NameLiteral)));
                    else
                        parseErrors.AddRange(collectionItemsSelectorPathElementResult.Errors);

                    return null;
                }

                return CollectionExpressionHelpers.Create(collectionItemsSelectorPathElementResult.Value);
            }

            if (bracesExpressionItem.Parameters.Count == 0)
            {
                parseErrors.Add(new JsonObjectParseError("Expected at least one index.", parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionItem)));
                return null;
            }

            var jsonValuePathElements = new List<IJsonValuePathElement>(2);

            if (bracesExpressionItem.NameLiteral != null)
                jsonValuePathElements.Add(new JsonValuePropertyNamePathElement(bracesExpressionItem.NameLiteral.LiteralName.Text,
                     parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem.NameLiteral)));

            var arrayIndexes = new List<IJsonArrayIndexInfo>(bracesExpressionItem.Parameters.Count);
            for (var i = 0; i < bracesExpressionItem.Parameters.Count; ++i)
            {
                var jsonPathIndexParameter = bracesExpressionItem.Parameters[i];

                if (jsonPathIndexParameter == null)
                {
                    parseErrors.Add(new JsonObjectParseError("Should be either json array index or json criteria for selecting array items.",
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem)));
                    return null;
                }

                if (jsonPathIndexParameter is INumericExpressionItem numericExpressionItem)
                {
                    if (numericExpressionItem.SucceededNumericTypeDescriptor.NumberTypeId == KnownNumericTypeDescriptorIds.IntegerValueId &&
                        int.TryParse(numericExpressionItem.Value.NumericValue, out var index) && index >= 0)
                    {
                        arrayIndexes.Add(new JsonArrayIndexInfo(index, parsedSimpleValue.LineInfo.GenerateRelativePosition(numericExpressionItem)));
                    }
                    else
                    {
                        parseErrors.Add(new JsonObjectParseError("Invalid array index. Expected a positive integer number for referenced array index.",
                            parsedSimpleValue.LineInfo.GenerateRelativePosition(jsonPathIndexParameter)));
                        return null;
                    }
                }
                else
                {
                    parseErrors.Add(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(jsonPathIndexParameter)));
                    return null;
                }
            }

            var jsonArrayIndexesPathElement = new JsonArrayIndexesPathElement(arrayIndexes);

            if (!ValidateIsNotPrecededWithCollectionSelector(currentJsonPathElements, jsonArrayIndexesPathElement, parseErrors,
                    "json array reference"))
                return null;

            jsonValuePathElements.Add(jsonArrayIndexesPathElement);
            return jsonValuePathElements;
        }

        return null;
    }

    private bool ValidateIsNotPrecededWithCollectionSelector(IReadOnlyList<IJsonValuePathElement> currentJsonPathElements,
        IJsonValuePathElement addedJsonValuePathElement, List<IJsonObjectParseError> parseErrors,
        string currentElementDescription)
    {
        if (currentJsonPathElements.Count > 0 && currentJsonPathElements[^1] is IJsonValueCollectionItemsSelectorPathElement {SelectsSingleItem: false} jsonValueCollectionItemsSelectorPathElement)
        {
            parseErrors.Add(new JsonObjectParseError($"Collection selector [{jsonValueCollectionItemsSelectorPathElement.FunctionName}]' cannot be followed by [{currentElementDescription}].",
                addedJsonValuePathElement.LineInfo));
            return false;
        }

        return true;
    }
}
