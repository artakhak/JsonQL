using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;
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

        if (operatorName == CustomJsonOperatorNames.AndNumbersAndReverseSign)
        {
            return CreateAndNumbersAndReverseSignOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);
        }

        return _defaultBinaryOperatorJsonFunctionFactory.GetBinaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operand1, operand2, jsonFunctionContext, operatorLineInfo);
    }

    private IParseResult<IJsonFunction> CreateAndNumbersAndReverseSignOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName, [operand1, operand2],
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new AndNumbersAndReverseSignOperatorFunction(operatorName,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }
}
