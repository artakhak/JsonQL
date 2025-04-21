using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for parsing a unary prefix operator expressions (e.g., "-Object1.Int1" or "!x[0].Bool1")) into a <see cref="IJsonFunction"/>.
/// </summary>
public interface IUnaryPrefixOperatorJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from unary prefix operator expression. 
    /// Example:[-Object1.Int1] or [!x[0].Bool1]
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed json value which contains the expression to be parsed.</param>
    /// <param name="operatorExpressionItem">Operator expression to convert to <see cref="IJsonFunction"/>.</param>
    /// <param name="jsonFunctionContext">If not null, parent function data.</param>
    /// <param name="operand">Operand.</param>
    /// <param name="operatorLineInfo">Operator line info.</param>
    /// <returns>
    /// Returns parse result.
    /// If the value <see cref="IParseResult{TValue}.Errors"/> is not empty, function failed to be parsed.
    /// Otherwise, the value <see cref="IParseResult{TValue}.Value"/> will be non-null, if function parsed, or null, if <see cref="IJsonFunction"/> does not
    /// know how to parse the expression into <see cref="IJsonFunction"/> (in which case the caller of this method will either try to parse the expression
    /// some other way, or will report an error).
    /// </returns>
    IParseResult<IJsonFunction> GetUnaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem,
        IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo);
}

public class UnaryPrefixOperatorJsonFunctionFactory : JsonFunctionFactoryAbstr, IUnaryPrefixOperatorJsonFunctionFactory
{
    /// <inheritdoc />
    public IParseResult<IJsonFunction> GetUnaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;

        switch (operatorName)
        {
            case JsonOperatorNames.NegativeValueOperator:
                return CreateNegativeValueOperatorFunction(parsedSimpleValue, operatorName, operand, jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.NegateOperator:
                return CreateNegateBooleanValueOperatorFunction(parsedSimpleValue, operatorName, operand, jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.TypeOfOperator:
                return CreateTypeOfJsonFunctionResultFunction(parsedSimpleValue, operatorName, operand, jsonFunctionContext, operatorLineInfo);
        }

        return OperatorJsonFunctionFactoryHelpers.GetGenericErrorResult(parsedSimpleValue, operatorExpressionItem);
    }

    private IParseResult<IJsonFunction> CreateNegativeValueOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand),
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new NegativeValueOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateNegateBooleanValueOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand),
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new NegateBooleanValueOperator(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateTypeOfJsonFunctionResultFunction(IParsedSimpleValue parsedSimpleValue, string operatorName,
        IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand),
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                ValidateIsNotMultipleValuesSelectorPath = false
            },
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 || parametersParseResult.Value == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new TypeOfJsonFunctionResultFunction(operatorName, parametersParseResult.Value, jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }
}