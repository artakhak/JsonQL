using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A custom factory for parsing <see cref="IBracesExpressionItem"/> (e.g., <b>Lower('EXAMPLE TEXT')</b>) into a <see cref="IJsonFunction"/>.
/// </summary>
public class CustomBracesJsonFunctionFactory: JsonFunctionFactoryAbstr, IBracesJsonFunctionFactory
{
    private readonly IBracesJsonFunctionFactory _defaultBracesJsonFunctionFactory;

    public CustomBracesJsonFunctionFactory(IBracesJsonFunctionFactory defaultBracesJsonFunctionFactory)
    {
        _defaultBracesJsonFunctionFactory = defaultBracesJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction> TryCreateBracesCustomFunction(IParsedSimpleValue parsedSimpleValue, IBracesExpressionItem bracesExpressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        var functionNameLiteralExpression = bracesExpressionItem.NameLiteral;

        if (functionNameLiteralExpression == null)
            return new ParseResult<IJsonFunction>([
                new JsonObjectParseError("Function name is missing",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem))
            ]);

        var functionName = functionNameLiteralExpression.LiteralName.Text;

        var functionParameters = bracesExpressionItem.Parameters;
        var functionLineInfo = parsedSimpleValue.LineInfo.GenerateRelativePosition(functionNameLiteralExpression);

        if (functionName == CustomJsonFunctionNames.ReverseTextAndAddMarkers)
        {
            return ReverseTextAndAddMarkersJsonFunction(parsedSimpleValue, functionName, functionParameters,  jsonFunctionContext, functionLineInfo);
        }

        return _defaultBracesJsonFunctionFactory.TryCreateBracesCustomFunction(parsedSimpleValue, bracesExpressionItem, jsonFunctionContext);
    }

    private IParseResult<IJsonFunction> ReverseTextAndAddMarkersJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = 
            JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonValuePathJsonFunction), true)
            {
                ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("addMarkers", typeof(IBooleanJsonFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = 
            new ReverseTextAndAddMarkersJsonFunction(functionName, parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2, jsonFunctionContext, functionLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }
}
