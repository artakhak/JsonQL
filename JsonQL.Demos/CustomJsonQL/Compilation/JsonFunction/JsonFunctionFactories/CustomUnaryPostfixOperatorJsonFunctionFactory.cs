using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// Custom factory for creating unary postfix operator functions.
/// Look at the default implementation <see cref="UnaryPostfixOperatorJsonFunctionFactory"/> for examples on how postfix operator
/// functions can be created.
/// </summary>
public class CustomUnaryPostfixOperatorJsonFunctionFactory : JsonFunctionFactoryAbstr, IUnaryPostfixOperatorJsonFunctionFactory
{
    private readonly IUnaryPostfixOperatorJsonFunctionFactory _defaultUnaryPostfixOperatorJsonFunctionFactory;

    public CustomUnaryPostfixOperatorJsonFunctionFactory(IUnaryPostfixOperatorJsonFunctionFactory defaultUnaryPostfixOperatorJsonFunctionFactory)
    {
        _defaultUnaryPostfixOperatorJsonFunctionFactory = defaultUnaryPostfixOperatorJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction> GetUnaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        string operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;

        if (operatorName == CustomJsonOperatorNames.DecrementByTwoPostfixOperator)
        {
            // TODO:..
            //return null!;
        }

        return _defaultUnaryPostfixOperatorJsonFunctionFactory.GetUnaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operand, jsonFunctionContext, operatorLineInfo);
    }
}