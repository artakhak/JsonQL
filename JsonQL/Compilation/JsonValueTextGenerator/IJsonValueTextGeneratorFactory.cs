using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonValueTextGenerator;

/// <summary>
/// Represents a factory for creating instances of text generators
/// that convert JSON value expressions to their string representations.
/// </summary>
public interface IJsonValueTextGeneratorFactory
{
    /// <summary>
    /// Creates an instance of <c>IParseResult</c> containing an <c>IJsonSimpleValueExpressionToStringConverter</c>.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value to be converted to a JSON-compatible text representation.</param>
    /// <param name="variablesManager">The manager responsible for resolving variable values used during the conversion.</param>
    /// <param name="expressionItem">The base expression item associated with the parsed value.</param>
    /// <returns>A result containing the object responsible for converting the parsed simple value to a string.</returns>
    IParseResult<IJsonSimpleValueExpressionToStringConverter> Create(IParsedSimpleValue parsedSimpleValue, IVariablesManager variablesManager, IExpressionItemBase expressionItem);
}

/// <inheritdoc />
public class JsonValueTextGeneratorFactory : IJsonValueTextGeneratorFactory
{
    private readonly IJsonFunctionFromExpressionParser _jsonFunctionFromExpressionParser;
    private readonly IStringFormatter _stringFormatter;

    /// <summary>
    /// Factory class for generating text representations of JSON values by creating instances of <c>IJsonSimpleValueExpressionToStringConverter</c>.
    /// </summary>
    /// <remarks>
    /// This class processes parsed JSON values, applies formatting, and resolves variable values to generate a string representation.
    /// </remarks>
    public JsonValueTextGeneratorFactory(
        IJsonFunctionFromExpressionParser jsonFunctionFromExpressionParser,
        IStringFormatter stringFormatter)
    {
        _jsonFunctionFromExpressionParser = jsonFunctionFromExpressionParser;
        _stringFormatter = stringFormatter;
    }

    /// <inheritdoc />
    public IParseResult<IJsonSimpleValueExpressionToStringConverter> Create(IParsedSimpleValue parsedSimpleValue, IVariablesManager variablesManager, IExpressionItemBase expressionItem)
    {
        var parseJsonFunctionResult = _jsonFunctionFromExpressionParser.Parse(parsedSimpleValue, expressionItem, new JsonFunctionValueEvaluationContext(variablesManager));

        if (parseJsonFunctionResult.Errors.Count > 0 || parseJsonFunctionResult.Value == null)
        {
            if (parseJsonFunctionResult.Errors.Count == 0)
                return new ParseResult<IJsonSimpleValueExpressionToStringConverter>(CollectionExpressionHelpers.Create(
                    new JsonObjectParseError("Failed to parse the expression",
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(expressionItem))
                ));

            return new ParseResult<IJsonSimpleValueExpressionToStringConverter>(parseJsonFunctionResult.Errors);
        }

        return new ParseResult<IJsonSimpleValueExpressionToStringConverter>(
            new JsonSimpleValueExpressionToStringConverter(_stringFormatter, parseJsonFunctionResult.Value));
    }
}