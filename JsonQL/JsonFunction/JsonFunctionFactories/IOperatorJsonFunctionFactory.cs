using JsonQL.JsonObjects;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for parsing <see cref="IOperatorExpressionItem"/> (e.g., "Object1.Int1 + 10") into a <see cref="IJsonFunction"/>.
/// </summary>
public interface IOperatorJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from operator expression. 
    /// Example:[x &lt; 10] 
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed json value which contains the expression to be parsed.</param>
    /// <param name="operatorExpressionItem">Operator expression to convert to <see cref="IJsonFunction"/>.</param>
    /// <param name="jsonFunctionContext">If not null, parent function data.</param>
    /// <returns>
    /// Returns parse result.
    /// If the value <see cref="IParseResult{TValue}.Errors"/> is not empty, function failed to be parsed.
    /// Otherwise, the value <see cref="IParseResult{TValue}.Value"/> will be non-null, if function parsed, or null, if <see cref="IJsonFunction"/> does not
    /// know how to parse the expression into <see cref="IJsonFunction"/> (in which case the caller of this method will either try to parse the expression
    /// some other way, or will report an error).
    /// </returns>
    IParseResult<IJsonFunction> TryCreateOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

public class OperatorJsonFunctionFactory : JsonFunctionFactoryAbstr, IOperatorJsonFunctionFactory
{
    private readonly IBinaryOperatorJsonFunctionFactory _binaryOperatorJsonFunctionFactory;
    private readonly IUnaryPrefixOperatorJsonFunctionFactory _prefixOperatorJsonFunctionFactory;
    private readonly IUnaryPostfixOperatorJsonFunctionFactory _postfixOperatorJsonFunctionFactory;

    public OperatorJsonFunctionFactory(
        IBinaryOperatorJsonFunctionFactory binaryOperatorJsonFunctionFactory,
        IUnaryPrefixOperatorJsonFunctionFactory prefixOperatorJsonFunctionFactory,
        IUnaryPostfixOperatorJsonFunctionFactory postfixOperatorJsonFunctionFactory)
    {
        _binaryOperatorJsonFunctionFactory = binaryOperatorJsonFunctionFactory;
        _prefixOperatorJsonFunctionFactory = prefixOperatorJsonFunctionFactory;
        _postfixOperatorJsonFunctionFactory = postfixOperatorJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction> TryCreateOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        var operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;
        var operatorLineInfo = parsedSimpleValue.LineInfo.GenerateRelativePosition(operatorExpressionItem);

        switch (operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.OperatorType)
        {
            case OperatorType.BinaryOperator:
                if (operatorExpressionItem.Operand2 == null)
                    return new ParseResult<IJsonFunction>(
                        CollectionExpressionHelpers.Create(new JsonObjectParseError($"The second binary operator operand is missing in operator [{operatorName}].",
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(operatorExpressionItem))));

                return _binaryOperatorJsonFunctionFactory.GetBinaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operatorExpressionItem.Operand1, operatorExpressionItem.Operand2,
                    jsonFunctionContext, operatorLineInfo);

            case OperatorType.PostfixUnaryOperator:
                return _postfixOperatorJsonFunctionFactory.GetUnaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operatorExpressionItem.Operand1,
                    jsonFunctionContext, operatorLineInfo);
            
            case OperatorType.PrefixUnaryOperator:
                return _prefixOperatorJsonFunctionFactory.GetUnaryOperatorFunction(parsedSimpleValue, operatorExpressionItem, operatorExpressionItem.Operand1,
                    jsonFunctionContext, operatorLineInfo);
        }

        return OperatorJsonFunctionFactoryHelpers.GetGenericErrorResult(parsedSimpleValue, operatorExpressionItem);
    }
}