// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Represents a specialized JSON value mutator designed to handle and transform simple JSON values.
/// This interface extends the functionality of <see cref="IJsonValueMutator"/> to provide specific
/// behavior for manipulating simple JSON elements such as strings, numbers, or booleans.
/// </summary>
public interface IJsonSimpleValueMutator : IJsonValueMutator
{

}
public class JsonSimpleValueMutator : JsonSimpleValueMutatorAbstr, IJsonSimpleValueMutator
{
    private readonly IReadOnlyList<IJsonSimpleValueExpressionToStringConverter> _parsedTextGeneratorExpressions;

    private readonly string _parsedValueTemplate;
   
    /// <summary>
    /// Represents a mutator for JSON simple values, allowing for the parsing of expressions within the values
    /// and the generation of corresponding text outputs based on those expressions.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed representation of the simple value.</param>
    /// <param name="parsedTextGeneratorExpressions">A collection of expressions used to generate text values.</param>
    /// <param name="parsedValueTemplate">
    /// A template generated from the original JSON value by replacing expressions embedded within '$' functions<br/>
    /// with placeholders. The placeholders allow for dynamic replacement at runtime using text generated from<br/>
    /// <paramref name="parsedTextGeneratorExpressions"/>.<br/>
    /// For example, if the JSON value is a simple value with expressions inside '$' functions,<br/>
    /// such as "http://$(AppSettings.HostName:localhost):$(AppSettings.Port:80)",<br/>
    /// then the value expressions will be parsed and stored in <see cref="parsedTextGeneratorExpressions"/>, and a<br/>
    /// templated value "http://{0}:{1}" will be generated and stored in <paramref name="parsedValueTemplate"/>.
    /// The placeholders '{0}' and '{1}' in this example will be later replaced with text<br/>
    /// generated from <see cref="parsedTextGeneratorExpressions"/>.
    /// </param>
    /// <remarks>
    /// This class is responsible for handling JSON simple values that may contain embedded expressions. It parses
    /// the value to extract expressions, constructs a template with placeholders, and ensures that text values
    /// are dynamically generated and substituted during runtime based on the extracted expressions.
    /// </remarks>
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

