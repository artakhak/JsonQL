using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
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
            // TODO:..
            //return null!;
        }

        return _defaultUnaryPrefixOperatorJsonFunctionFactory.GetUnaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operand, jsonFunctionContext, operatorLineInfo);
    }
}