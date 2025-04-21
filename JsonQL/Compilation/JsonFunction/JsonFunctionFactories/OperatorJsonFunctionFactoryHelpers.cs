using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

public static class OperatorJsonFunctionFactoryHelpers
{
    public static ParseResult<IJsonFunction> GetGenericErrorResult(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem)
    {
        return new ParseResult<IJsonFunction>(CollectionExpressionHelpers.Create(new JsonObjectParseError("Failed to parse an operator expression into json function.",
            parsedSimpleValue.LineInfo.GenerateRelativePosition(operatorExpressionItem))));
    }
}