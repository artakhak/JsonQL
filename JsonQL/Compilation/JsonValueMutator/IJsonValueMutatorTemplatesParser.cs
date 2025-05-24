using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using TextParser;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Defines an interface for parsing JSON value mutator function templates from parsed JSON data.
/// </summary>
/// <remarks>
/// Implementations are responsible for interpreting JSON value strings as potential mutator functions
/// and extracting structured expressions templates for further processing.
/// </remarks>
public interface IJsonValueMutatorFunctionTemplatesParser
{
    /// <summary>
    /// Tries to parse the value of <see cref="IParsedSimpleValue"/> in the provided <paramref name="parsedSimpleValue"/>
    /// into a list of mutator function expression templates.
    /// </summary>
    /// <param name="jsonObjectData">An object containing data about the parsed JSON.</param>
    /// <param name="parsedSimpleValue">The JSON value with the associated value to be parsed.</param>
    /// <returns>
    /// A result containing a list of parsed expression data. If no mutator function expression templates
    /// are found, the returned list will be empty.
    /// </returns>
    IParseResult<IReadOnlyList<IParsedExpressionData>> TryParseExpression(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue);
}

/// <inheritdoc />
public class JsonValueMutatorFunctionTemplatesParser : IJsonValueMutatorFunctionTemplatesParser
{
    private readonly IExpressionParser _expressionParser;
    private readonly ITextSymbolsParserFactory _textSymbolsParserFactory;

    /// <summary>
    /// Responsible for parsing JSON value mutator function templates into expression templates using
    /// specified text and expression parsers.
    /// </summary>
    public JsonValueMutatorFunctionTemplatesParser(
        IJsonQLExpressionParserFactory jsonQLExpressionParserFactory,
        ITextSymbolsParserFactory textSymbolsParserFactory)
    {
        _expressionParser = jsonQLExpressionParserFactory.Create();
        _textSymbolsParserFactory = textSymbolsParserFactory;
    }

    private bool IsValidMutatorFunctionCharacter(char character, int positionInLiteral, ITextSymbolsParserState textSymbolsParserState)
    {
        if (positionInLiteral == 0)
            return character == '$';

        return Helpers.IsLatinLetter(character);
    }

    /// <inheritdoc />
    public IParseResult<IReadOnlyList<IParsedExpressionData>> TryParseExpression(IJsonObjectData jsonObjectData, IParsedSimpleValue parsedSimpleValue)
    {
        if (parsedSimpleValue.Value == null)
            throw new ArgumentException($"The value of [{nameof(IParsedSimpleValue.Value)}] cannot be null.", nameof(parsedSimpleValue));

        var errors = new List<IJsonObjectParseError>();

        List<IParsedExpressionData> parsedExpressions = new();

        var parsedText = parsedSimpleValue.Value;

        var textSymbolsParser = _textSymbolsParserFactory.CreateTextSymbolsParser(parsedText, IsValidMutatorFunctionCharacter);

        string GetMutatorFunctionExpressionMissingError(string mutatorFunctionName) => $"Expression is missing in mutator function [{mutatorFunctionName}].";

        while (!textSymbolsParser.IsEndOfTextReached)
        {
            var indexOfMutatorFunctionStartCharacter = parsedText.IndexOf('$', textSymbolsParser.PositionInText);

            if (indexOfMutatorFunctionStartCharacter < 0)
                break;

            textSymbolsParser.MoveToPosition(indexOfMutatorFunctionStartCharacter);
            var templateStartIndex = textSymbolsParser.PositionInText;

            if (!textSymbolsParser.ReadSymbol())
                break;

            var expressionStartIndex = textSymbolsParser.PositionInText;

            var mutatorFunctionName = textSymbolsParser.LastReadSymbol;

            if (!textSymbolsParser.SkipSpaces())
            {
                errors.Add(new JsonObjectParseError(GetMutatorFunctionExpressionMissingError(mutatorFunctionName),
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionStartIndex)));
                return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
            }

            expressionStartIndex = textSymbolsParser.PositionInText;

            IParseExpressionResult? parsedExpressionResult;
            bool usesOpeningBrace = false;
            if (textSymbolsParser.CurrentChar == '(')
            {
                usesOpeningBrace = true;
                parsedExpressionResult = _expressionParser.ParseBracesExpression(Constants.JsonExpressionLanguageName,
                    parsedText,
                    new ParseExpressionOptions
                    {
                        StartIndex = expressionStartIndex
                    });
            }
            else
            {
                if (mutatorFunctionName.Length == 1)
                {
                    errors.Add(new JsonObjectParseError(
                        $"Mutator function [{mutatorFunctionName}] should be followed by opening brace '('",
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(textSymbolsParser.PositionInText)));
                    return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
                }

                if (!char.IsWhiteSpace(parsedText[templateStartIndex + mutatorFunctionName.Length]))
                {
                    errors.Add(new JsonObjectParseError(
                        $"{ParseErrorsConstants.InvalidSymbol}: Mutator function [{mutatorFunctionName}] should be followed by either space of opening brace '('",
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(textSymbolsParser.PositionInText)));
                    return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
                }

                parsedExpressionResult = _expressionParser.ParseExpression(Constants.JsonExpressionLanguageName,
                    parsedText,
                    new ParseExpressionOptions
                    {
                        StartIndex = expressionStartIndex
                    });
            }

            if (parsedExpressionResult.ParseErrorData.AllParseErrorItems.Count > 0)
            {
                errors.AddRange(CompilationHelpers.ConvertToJsonObjectParseError(jsonObjectData, parsedSimpleValue,
                    parsedExpressionResult.ParseErrorData.AllParseErrorItems));
                return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
            }

            if (parsedExpressionResult.RootExpressionItem.Children.Count == 0)
            {
                errors.Add(new JsonObjectParseError(GetMutatorFunctionExpressionMissingError(mutatorFunctionName),
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionStartIndex)));
                return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
            }

            if (parsedExpressionResult.RootExpressionItem.Children.Count > 1)
            {
                errors.Add(new JsonObjectParseError(
                    $"Invalid expression in function [{mutatorFunctionName}].",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionStartIndex)));
                return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
            }

            var parsedExpressionItem = parsedExpressionResult.RootExpressionItem.Children[0];

            if (usesOpeningBrace && parsedExpressionItem is IBracesExpressionItem bracesExpressionItem)
            {
                if (bracesExpressionItem.Parameters.Count == 0)
                {
                    errors.Add(new JsonObjectParseError(
                        GetMutatorFunctionExpressionMissingError(mutatorFunctionName),
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem.OpeningBrace.IndexInText + 1)));
                    return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
                }

                if ( bracesExpressionItem.Parameters.Count > 1)
                {
                    errors.Add(new JsonObjectParseError(
                        ParseErrorsConstants.InvalidSymbol,
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem.Parameters[1]!.IndexInText)));
                    return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
                }

                var bracesExpressionItemParameter = bracesExpressionItem.Parameters[0];

                if (bracesExpressionItemParameter == null)
                {
                    errors.Add(new JsonObjectParseError(
                        ParseErrorsConstants.InvalidSymbol,
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem.IndexInText + 1)));
                    return new ParseResult<IReadOnlyList<IParsedExpressionData>>(errors);
                }

                parsedExpressionItem = bracesExpressionItemParameter;
            }

            parsedExpressions.Add(
                new ParsedExpressionData(mutatorFunctionName, templateStartIndex, parsedExpressionResult.PositionInTextOnCompletion - templateStartIndex,
                    parsedExpressionItem));

            textSymbolsParser.MoveToPosition(parsedExpressionResult.PositionInTextOnCompletion);
        }

        return new ParseResult<IReadOnlyList<IParsedExpressionData>>(parsedExpressions);
    }
}