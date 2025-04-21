using JsonQL.Compilation.JsonFunction;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonValueTextGenerator;

public interface IJsonValueTextGeneratorFactory
{
    IParseResult<IJsonSimpleValueExpressionToStringConverter> Create(IParsedSimpleValue parsedSimpleValue, IVariablesManager variablesManager, IExpressionItemBase expressionItem);
}

/// <inheritdoc />
public class JsonValueTextGeneratorFactory : IJsonValueTextGeneratorFactory
{
    private readonly IJsonFunctionFromExpressionParser _jsonFunctionFromExpressionParser;
    private readonly IStringFormatter _stringFormatter;

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