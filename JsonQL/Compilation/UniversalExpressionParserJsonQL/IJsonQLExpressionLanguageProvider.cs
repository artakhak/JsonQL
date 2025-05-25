using TextParser;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems.Custom;

namespace JsonQL.Compilation.UniversalExpressionParserJsonQL;

/// <summary>
/// <see cref="IExpressionLanguageProvider"/> for JsonQL, enabling the parsing of JSON-like expressions.
/// Defines the rules and characteristics of the JSON expression language. It includes properties for
/// language-specific features such as supported constant markers, operators, and other language-specific rules.
/// </summary>
public interface IJsonQLExpressionLanguageProvider : IExpressionLanguageProvider
{

}

/// <summary>
/// Provides implementation for the JSON expression language, enabling the parsing of JSON-like expressions.
/// Implements the <see cref="IJsonQLExpressionLanguageProvider"/> interface.
/// </summary>
/// <remarks>
/// This class defines the rules and characteristics of the JSON expression language. It includes properties for
/// language-specific features such as supported constant markers, operators, and other language-specific rules.
/// </remarks>
public class JsonQLExpressionLanguageProvider : IJsonQLExpressionLanguageProvider
{
    private readonly List<IOperatorInfo> _operators;

    /// <summary>
    /// Constructor.
    /// </summary>
    public JsonQLExpressionLanguageProvider()
    {
        _operators = new List<IOperatorInfo>
        {
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.JsonPathSeparator), OperatorType.BinaryOperator, 0),

            new OperatorInfoWithAutoId(JsonOperatorNames.NegateOperator, OperatorType.PrefixUnaryOperator, 100),
            new OperatorInfoWithAutoId(JsonOperatorNames.NegativeValueOperator, OperatorType.PrefixUnaryOperator, 100),
            new OperatorInfoWithAutoId(JsonOperatorNames.TypeOfOperator, OperatorType.PrefixUnaryOperator, 100),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.DefaultValueOperator), OperatorType.BinaryOperator, 200),

            new OperatorInfoWithAutoId(JsonOperatorNames.IsUndefinedOperator, OperatorType.PostfixUnaryOperator, 300),
            new OperatorInfoWithAutoId(JsonOperatorNames.IsNotUndefinedOperator, OperatorType.PostfixUnaryOperator, 300),

            new OperatorInfoWithAutoId(JsonOperatorNames.IsNullOperator, OperatorType.PostfixUnaryOperator, 300),
            new OperatorInfoWithAutoId(JsonOperatorNames.IsNotNullOperator, OperatorType.PostfixUnaryOperator, 300),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.AssertNotNull), OperatorType.PostfixUnaryOperator, 300),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.ContainsOperator), OperatorType.BinaryOperator, 300),
            new OperatorInfoWithAutoId(JsonOperatorNames.StartsWith, OperatorType.BinaryOperator, 300),
            new OperatorInfoWithAutoId(JsonOperatorNames.EndsWith, OperatorType.BinaryOperator, 300),
            
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.MultiplyOperator), OperatorType.BinaryOperator, 400),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.DivideOperator), OperatorType.BinaryOperator, 400),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.QuotientOperator), OperatorType.BinaryOperator, 500),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.AddOperator), OperatorType.BinaryOperator, 500),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.SubtractOperator), OperatorType.BinaryOperator, 500),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.EqualsOperator), OperatorType.BinaryOperator, 600),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.NotEqualsOperator), OperatorType.BinaryOperator, 600),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.LessThanOperator), OperatorType.BinaryOperator, 600),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.LessThanOrEqualOperator), OperatorType.BinaryOperator, 600),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.GreaterThanOperator), OperatorType.BinaryOperator, 600),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.GreaterThanOrEqualOperator), OperatorType.BinaryOperator, 600),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.AndOperator), OperatorType.BinaryOperator, 700),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.OrOperator), OperatorType.BinaryOperator, 800),

            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.LambdaOperator), OperatorType.BinaryOperator, 10000),
            new OperatorInfoWithAutoId(CollectionExpressionHelpers.Create(JsonOperatorNames.JsonFunctionParameterValueOperator), OperatorType.BinaryOperator, 20100)
        };
    }

    /// <inheritdoc />
    public string LanguageName => Constants.JsonExpressionLanguageName;

    /// <inheritdoc />
    public string Description => "Parses Json expressions";

    /// <inheritdoc />
    public string? LineCommentMarker => null;

    /// <inheritdoc />
    public string? MultilineCommentStartMarker => null;

    /// <inheritdoc />
    public string? MultilineCommentEndMarker => null;

    /// <inheritdoc />
    public char ExpressionSeparatorCharacter => '\0';

    /// <inheritdoc />
    public string? CodeBlockStartMarker => null;

    /// <inheritdoc />
    public string? CodeBlockEndMarker => null;

    /// <inheritdoc />
    public IReadOnlyList<char> ConstantTextStartEndMarkerCharacters { get; } = new List<char>
    {
        '\'', '\"', '`'
    };

    /// <inheritdoc />
    public IReadOnlyList<IOperatorInfo> Operators => _operators;

    /// <inheritdoc />
    public IReadOnlyList<ILanguageKeywordInfo> Keywords { get; } = Array.Empty<ILanguageKeywordInfo>();

    /// <inheritdoc />
    public bool IsValidLiteralCharacter(char character, int positionInLiteral, ITextSymbolsParserState? textSymbolsParserState)
    {
        if (character == '_')
            return true;

        if (//character == '.' ||
            Char.IsNumber(character))
            return positionInLiteral > 0;

        return Helpers.IsLatinLetter(character);
    }

    /// <inheritdoc />
    public bool IsLanguageCaseSensitive => true;

    /// <inheritdoc />
    public IEnumerable<ICustomExpressionItemParser> CustomExpressionItemParsers { get; } = Array.Empty<ICustomExpressionItemParser>();

    /// <inheritdoc />
    public IReadOnlyList<NumericTypeDescriptor> NumericTypeDescriptors => ExpressionLanguageProviderHelpers.GetDefaultNumericTypeDescriptors();

    /// <inheritdoc />
    public bool SupportsPrefixes => false;

    /// <inheritdoc />
    public bool SupportsKeywords => false;

    public IEnumerable<char> SpecialOperatorCharacters
    {
        get
        {
            foreach (var specialOperatorCharacter in DefaultSpecialCharacters.SpecialOperatorCharacters)
            {
                yield return specialOperatorCharacter;
            }

            yield return '.';
        }
    }
    public IEnumerable<char> SpecialNonOperatorCharacters => DefaultSpecialCharacters.SpecialNonOperatorCharacters;
}