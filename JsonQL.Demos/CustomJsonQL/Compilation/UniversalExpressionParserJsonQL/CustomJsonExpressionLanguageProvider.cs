using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using TextParser;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems.Custom;

namespace JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;

/// <summary>
/// Custom implementation of <see cref="IExpressionLanguageProvider"/> for JsonQL expressions. 
/// </summary>
public class CustomJsonExpressionLanguageProvider: IExpressionLanguageProvider
{
    private readonly IExpressionLanguageProvider _defaultJsonExpressionLanguageProvider;

    private readonly List<IOperatorInfo> _operators;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="defaultJsonExpressionLanguageProvider">Decorated implementation of <see cref="IExpressionLanguageProvider"/>.
    /// Will be used for most implementations of <see cref="IExpressionLanguageProvider"/>.
    /// An instance of default JsonQL implementation <see cref="JsonQLExpressionLanguageProvider"/> can be used for this parameter.</param>
    public CustomJsonExpressionLanguageProvider(IExpressionLanguageProvider defaultJsonExpressionLanguageProvider)
    {
        _defaultJsonExpressionLanguageProvider = defaultJsonExpressionLanguageProvider;

        _operators = new List<IOperatorInfo>(_defaultJsonExpressionLanguageProvider.Operators.Count + 10);

        _operators.AddRange(_defaultJsonExpressionLanguageProvider.Operators);

        _operators.Add(new OperatorInfoWithAutoId(JsonOperatorNames.NegateOperator, OperatorType.PrefixUnaryOperator, 100));
    }

    public bool IsValidLiteralCharacter(char character, int positionInLiteral, ITextSymbolsParserState textSymbolsParserState)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string LanguageName => _defaultJsonExpressionLanguageProvider.LanguageName;

    /// <inheritdoc />
    public string Description => _defaultJsonExpressionLanguageProvider.Description;

    /// <inheritdoc />
    public string LineCommentMarker => _defaultJsonExpressionLanguageProvider.LineCommentMarker;

    /// <inheritdoc />
    public string MultilineCommentStartMarker => _defaultJsonExpressionLanguageProvider.MultilineCommentStartMarker;

    /// <inheritdoc />
    public string MultilineCommentEndMarker => _defaultJsonExpressionLanguageProvider.MultilineCommentEndMarker;

    /// <inheritdoc />
    public char ExpressionSeparatorCharacter => _defaultJsonExpressionLanguageProvider.ExpressionSeparatorCharacter;

    /// <inheritdoc />
    public string CodeBlockStartMarker => _defaultJsonExpressionLanguageProvider.CodeBlockStartMarker;

    /// <inheritdoc />
    public string CodeBlockEndMarker => _defaultJsonExpressionLanguageProvider.CodeBlockEndMarker;

    /// <inheritdoc />
    public IReadOnlyList<char> ConstantTextStartEndMarkerCharacters => _defaultJsonExpressionLanguageProvider.ConstantTextStartEndMarkerCharacters;

    /// <inheritdoc />
    public IReadOnlyList<IOperatorInfo> Operators => _operators;

    /// <inheritdoc />
    public IReadOnlyList<ILanguageKeywordInfo> Keywords => _defaultJsonExpressionLanguageProvider.Keywords;

    /// <inheritdoc />
    public bool IsLanguageCaseSensitive => _defaultJsonExpressionLanguageProvider.IsLanguageCaseSensitive;

    /// <inheritdoc />
    public IEnumerable<ICustomExpressionItemParser> CustomExpressionItemParsers => _defaultJsonExpressionLanguageProvider.CustomExpressionItemParsers;

    /// <inheritdoc />
    public IReadOnlyList<NumericTypeDescriptor> NumericTypeDescriptors => _defaultJsonExpressionLanguageProvider.NumericTypeDescriptors;

    /// <inheritdoc />
    public bool SupportsPrefixes => _defaultJsonExpressionLanguageProvider.SupportsPrefixes;

    /// <inheritdoc />
    public bool SupportsKeywords => _defaultJsonExpressionLanguageProvider.SupportsKeywords;

    /// <inheritdoc />
    public IEnumerable<char> SpecialOperatorCharacters => _defaultJsonExpressionLanguageProvider.SpecialOperatorCharacters;

    /// <inheritdoc />
    public IEnumerable<char> SpecialNonOperatorCharacters => _defaultJsonExpressionLanguageProvider.SpecialNonOperatorCharacters;
}