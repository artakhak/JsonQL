using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using TextParser;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems.Custom;

namespace JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;

/// <summary>
/// Custom implementation of <see cref="IJsonQLExpressionLanguageProvider"/> for JsonQL expressions. 
/// </summary>
public class CustomJsonExpressionLanguageProvider: IJsonQLExpressionLanguageProvider
{
    private readonly IJsonQLExpressionLanguageProvider _defaultJsonQLExpressionLanguageProvider;

    private readonly List<IOperatorInfo> _operators;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="defaultJsonQLExpressionLanguageProvider">
    /// Decorated implementation of <see cref="IJsonQLExpressionLanguageProvider"/>.
    /// An instance of default JsonQL implementation <see cref="JsonQLExpressionLanguageProvider"/> can be used for this parameter. 
    /// </param>
    public CustomJsonExpressionLanguageProvider(IJsonQLExpressionLanguageProvider defaultJsonQLExpressionLanguageProvider)
    {
        _defaultJsonQLExpressionLanguageProvider = defaultJsonQLExpressionLanguageProvider;

        _operators = new List<IOperatorInfo>(_defaultJsonQLExpressionLanguageProvider.Operators.Count + 10);

        _operators.AddRange(_defaultJsonQLExpressionLanguageProvider.Operators);

        _operators.Add(new OperatorInfoWithAutoId(CustomJsonOperatorNames.IncrementByTwoPrefixOperator, OperatorType.PrefixUnaryOperator, 100));
        _operators.Add(new OperatorInfoWithAutoId(CustomJsonOperatorNames.IsEvenPostfixOperators, OperatorType.PostfixUnaryOperator, 100));
        _operators.Add(new OperatorInfoWithAutoId(CustomJsonOperatorNames.AndNumbersAndReverseSign, OperatorType.BinaryOperator, 500));
    }

    public bool IsValidLiteralCharacter(char character, int positionInLiteral, ITextSymbolsParserState textSymbolsParserState)
    {
        return _defaultJsonQLExpressionLanguageProvider.IsValidLiteralCharacter(character, positionInLiteral, textSymbolsParserState);
    }

    /// <inheritdoc />
    public string LanguageName => _defaultJsonQLExpressionLanguageProvider.LanguageName;

    /// <inheritdoc />
    public string Description => _defaultJsonQLExpressionLanguageProvider.Description;

    /// <inheritdoc />
    public string LineCommentMarker => _defaultJsonQLExpressionLanguageProvider.LineCommentMarker;

    /// <inheritdoc />
    public string MultilineCommentStartMarker => _defaultJsonQLExpressionLanguageProvider.MultilineCommentStartMarker;

    /// <inheritdoc />
    public string MultilineCommentEndMarker => _defaultJsonQLExpressionLanguageProvider.MultilineCommentEndMarker;

    /// <inheritdoc />
    public char ExpressionSeparatorCharacter => _defaultJsonQLExpressionLanguageProvider.ExpressionSeparatorCharacter;

    /// <inheritdoc />
    public string CodeBlockStartMarker => _defaultJsonQLExpressionLanguageProvider.CodeBlockStartMarker;

    /// <inheritdoc />
    public string CodeBlockEndMarker => _defaultJsonQLExpressionLanguageProvider.CodeBlockEndMarker;

    /// <inheritdoc />
    public IReadOnlyList<char> ConstantTextStartEndMarkerCharacters => _defaultJsonQLExpressionLanguageProvider.ConstantTextStartEndMarkerCharacters;

    /// <inheritdoc />
    public IReadOnlyList<IOperatorInfo> Operators => _operators;

    /// <inheritdoc />
    public IReadOnlyList<ILanguageKeywordInfo> Keywords => _defaultJsonQLExpressionLanguageProvider.Keywords;

    /// <inheritdoc />
    public bool IsLanguageCaseSensitive => _defaultJsonQLExpressionLanguageProvider.IsLanguageCaseSensitive;

    /// <inheritdoc />
    public IEnumerable<ICustomExpressionItemParser> CustomExpressionItemParsers => _defaultJsonQLExpressionLanguageProvider.CustomExpressionItemParsers;

    /// <inheritdoc />
    public IReadOnlyList<NumericTypeDescriptor> NumericTypeDescriptors => _defaultJsonQLExpressionLanguageProvider.NumericTypeDescriptors;

    /// <inheritdoc />
    public bool SupportsPrefixes => _defaultJsonQLExpressionLanguageProvider.SupportsPrefixes;

    /// <inheritdoc />
    public bool SupportsKeywords => _defaultJsonQLExpressionLanguageProvider.SupportsKeywords;

    /// <inheritdoc />
    public IEnumerable<char> SpecialOperatorCharacters => _defaultJsonQLExpressionLanguageProvider.SpecialOperatorCharacters;

    /// <inheritdoc />
    public IEnumerable<char> SpecialNonOperatorCharacters => _defaultJsonQLExpressionLanguageProvider.SpecialNonOperatorCharacters;
}
