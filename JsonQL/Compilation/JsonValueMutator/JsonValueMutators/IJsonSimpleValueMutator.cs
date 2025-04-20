using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

public interface IJsonSimpleValueMutator : IJsonValueMutator
{

}
public class JsonSimpleValueMutator : JsonSimpleValueMutatorAbstr, IJsonSimpleValueMutator
{
    private readonly IReadOnlyList<IJsonSimpleValueExpressionToStringConverter> _parsedTextGeneratorExpressions;

    private readonly string _parsedValueTemplate;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parsedSimpleValue"></param>
    /// <param name="parsedTextGeneratorExpressions">List of text generator expressions.</param>
    /// <param name="parsedValueTemplate">
    /// If the json value is a simple value with expressions inside '$' functions,<br/>
    /// such as "http://$(AppSettings.HostName:localhost):$(AppSettings.Port:80)",
    /// then the value expressions will be parsed and stored in <see cref="parsedTextGeneratorExpressions"/>, and a<br/>
    /// templated value will be generated with placeholders, as<br/>
    /// "http://{0}:{1}" for the example provided.
    /// The placeholders '{0}' and '{1}' in this example will be later replaced with text<br/>
    /// generated from <see cref="parsedTextGeneratorExpressions"/>.<br/>
    /// The value is null, if no 'value' elements are used.
    /// </param>
    public JsonSimpleValueMutator(IParsedSimpleValue parsedSimpleValue,
        IReadOnlyList<IJsonSimpleValueExpressionToStringConverter> parsedTextGeneratorExpressions, string parsedValueTemplate) : base(parsedSimpleValue, null)
    {
        _parsedTextGeneratorExpressions = parsedTextGeneratorExpressions;
        _parsedValueTemplate = parsedValueTemplate;
    }

    /// <inheritdoc />
    protected override IParseResult<string> GenerateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues)
    {
        var generatedText = _parsedValueTemplate;

        for (var i = 0; i < _parsedTextGeneratorExpressions.Count; ++i)
        {
            var result = _parsedTextGeneratorExpressions[i].GenerateStringValue(rootParsedValue, compiledParentRootParsedValues);

            if (result.Errors.Count > 0)
                return result;

            generatedText = generatedText.Replace(string.Concat('{', i, '}'), result.Value);
        }

        return new ParseResult<string>(generatedText);
    }
}

