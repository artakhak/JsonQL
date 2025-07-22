using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// Custom factory for creating unary prefix operator functions.
/// Look at the default implementation <see cref="UnaryPrefixOperatorJsonFunctionFactory"/> for examples on how prefix operator
/// functions can be created.
/// </summary>
public class CustomUnaryPrefixOperatorJsonFunctionFactory : JsonFunctionFactoryAbstr, IUnaryPrefixOperatorJsonFunctionFactory
{
    private readonly IUnaryPrefixOperatorJsonFunctionFactory _defaultUnaryPrefixOperatorJsonFunctionFactory;

    public CustomUnaryPrefixOperatorJsonFunctionFactory(IUnaryPrefixOperatorJsonFunctionFactory defaultUnaryPrefixOperatorJsonFunctionFactory)
    {
        _defaultUnaryPrefixOperatorJsonFunctionFactory = defaultUnaryPrefixOperatorJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction> GetUnaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        string operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;

        if (operatorName == CustomJsonOperatorNames.IncrementByTwoPrefixOperator)
        {
            return CreateIncrementByTwoPrefixOperatorFunction(parsedSimpleValue, operatorName, operand, jsonFunctionContext, operatorLineInfo);
        }

        return _defaultUnaryPrefixOperatorJsonFunctionFactory.GetUnaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operand, jsonFunctionContext, operatorLineInfo);
    }

    private IParseResult<IJsonFunction> CreateIncrementByTwoPrefixOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, operatorName, [operand],
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new IncrementByTwoPrefixOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }
}
