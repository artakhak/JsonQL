using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// Custom factory for creating binary operator functions.
/// Look at the default implementation <see cref="BinaryOperatorJsonFunctionFactory"/> for examples on how binary operator
/// functions can be created.
/// </summary>
public class CustomBinaryOperatorJsonFunctionFactory: JsonFunctionFactoryAbstr, IBinaryOperatorJsonFunctionFactory
{
    private readonly IBinaryOperatorJsonFunctionFactory _defaultBinaryOperatorJsonFunctionFactory;

    public CustomBinaryOperatorJsonFunctionFactory(IBinaryOperatorJsonFunctionFactory defaultBinaryOperatorJsonFunctionFactory)
    {
        _defaultBinaryOperatorJsonFunctionFactory = defaultBinaryOperatorJsonFunctionFactory;
    }

    public IParseResult<IJsonFunction> GetBinaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IExpressionItemBase operand1, IExpressionItemBase operand2, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        string operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;

        if (operatorName == CustomJsonOperatorNames.AddAndIncrementByTwo)
        {
            //return null!;
        }

        return _defaultBinaryOperatorJsonFunctionFactory.GetBinaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operand1, operand2, jsonFunctionContext, operatorLineInfo);
    }
}