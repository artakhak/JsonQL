using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// Custom factory for creating unary postfix operator functions.
/// Look at the default implementation <see cref="UnaryPostfixOperatorJsonFunctionFactory"/> for examples on how postfix operator
/// functions can be created.
/// </summary>
public class CustomUnaryPostfixOperatorJsonFunctionFactory : JsonFunctionFactoryAbstr, IUnaryPostfixOperatorJsonFunctionFactory
{
    private static readonly string IsEvenOperatorName = Helpers.GetOperatorName(CustomJsonOperatorNames.IsEvenPostfixOperators);

    private readonly IUnaryPostfixOperatorJsonFunctionFactory _defaultUnaryPostfixOperatorJsonFunctionFactory;

    public CustomUnaryPostfixOperatorJsonFunctionFactory(IUnaryPostfixOperatorJsonFunctionFactory defaultUnaryPostfixOperatorJsonFunctionFactory)
    {
        _defaultUnaryPostfixOperatorJsonFunctionFactory = defaultUnaryPostfixOperatorJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction> GetUnaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;

        if (operatorName == IsEvenOperatorName)
        {
            return CreateIsEvenOperatorFunction(parsedSimpleValue, operatorName, operand, jsonFunctionContext, operatorLineInfo);
        }

        return _defaultUnaryPostfixOperatorJsonFunctionFactory.GetUnaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operand, jsonFunctionContext, operatorLineInfo);
    }

    private IParseResult<IJsonFunction> CreateIsEvenOperatorFunction(IParsedSimpleValue parsedSimpleValue,
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

        parametersJsonFunctionContext.ParentJsonFunction = new IsEvenPostfixOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }
}
