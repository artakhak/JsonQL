using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for parsing a unary postfix operator expressions (e.g., "x[0].Any(y => y assert > 3)" or "x[0].Any(y => (y > 3) assert)", where assert is the postfix operator that asserts that the value ios not null) into a <see cref="IJsonFunction"/>.
/// </summary>
public interface IUnaryPostfixOperatorJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from unary prefix operator expression. 
    /// Example: "x[0].Any(y => y assert > 3)" or "x[0].Any(y => (y > 3) assert)"
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

public class UnaryPostfixOperatorJsonFunctionFactory : JsonFunctionFactoryAbstr, IUnaryPostfixOperatorJsonFunctionFactory
{
    private static readonly string IsNullOperatorName = UniversalExpressionParser.Helpers.GetOperatorName(JsonOperatorNames.IsNullOperator);
    private static readonly string IsNotNullOperatorName = UniversalExpressionParser.Helpers.GetOperatorName(JsonOperatorNames.IsNotNullOperator);

    private static readonly string IsUndefinedOperatorName = UniversalExpressionParser.Helpers.GetOperatorName(JsonOperatorNames.IsUndefinedOperator);
    private static readonly string IsNotUndefinedOperatorName = UniversalExpressionParser.Helpers.GetOperatorName(JsonOperatorNames.IsNotUndefinedOperator);

    private readonly IAssertOperatorFunctionFactory _assertOperatorFunctionFactory;

    public UnaryPostfixOperatorJsonFunctionFactory(IAssertOperatorFunctionFactory assertOperatorFunctionFactory)
    {
        _assertOperatorFunctionFactory = assertOperatorFunctionFactory;
    }
    /// <inheritdoc />
    public IParseResult<IJsonFunction> GetUnaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IExpressionItemBase operand, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;

        if (operatorName == IsNullOperatorName)
            return CreateIsNullOperatorFunction(operatorName, true, parsedSimpleValue, operand, jsonFunctionContext, operatorLineInfo);

        if (operatorName == IsNotNullOperatorName)
            return CreateIsNullOperatorFunction(operatorName, false, parsedSimpleValue, operand, jsonFunctionContext, operatorLineInfo);

        if (operatorName == IsUndefinedOperatorName)
            return CreateIsUndefinedNotUndefinedOperatorFunction(operatorName, true, parsedSimpleValue, operand, jsonFunctionContext, operatorLineInfo);

        if (operatorName == IsNotUndefinedOperatorName)
            return CreateIsUndefinedNotUndefinedOperatorFunction(operatorName, false, parsedSimpleValue, operand, jsonFunctionContext, operatorLineInfo);

        switch (operatorName)
        {
            case JsonOperatorNames.AssertNotNull:
                return CreateAssertNotNullOperatorFunction(parsedSimpleValue, operatorName, operand, jsonFunctionContext, operatorLineInfo);
        }

        return OperatorJsonFunctionFactoryHelpers.GetGenericErrorResult(parsedSimpleValue, operatorExpressionItem);
    }

    private IParseResult<IJsonFunction> CreateAssertNotNullOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1),
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction =
            _assertOperatorFunctionFactory.CreateAssertOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IBooleanJsonFunction> CreateIsNullOperatorFunction(string operatorName, bool isNullOperator, IParsedSimpleValue parsedSimpleValue,
        IExpressionItemBase operand1, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonValuePathJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1),
            new JsonFunctionParameterMetadata("value", typeof(IJsonValuePathJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IsNullOperatorFunction>(parametersParseResult.Errors);

        IBooleanJsonFunction isNullOperatorFunction = isNullOperator ?
            new IsNullOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo) :
            new IsNotNullOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo);

        parametersJsonFunctionContext.ParentJsonFunction = isNullOperatorFunction;

        return new ParseResult<IBooleanJsonFunction>(isNullOperatorFunction);
    }
  
    private IParseResult<IJsonFunction> CreateIsUndefinedNotUndefinedOperatorFunction(string operatorName, bool isUndefinedOperator, IParsedSimpleValue parsedSimpleValue,
        IExpressionItemBase operand1,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1),
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                ValidateIsNotMultipleValuesSelectorPath = false
            },
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);
       
        parametersJsonFunctionContext.ParentJsonFunction = isUndefinedOperator ?
            new IsUndefinedOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo) : 
            new IsNotUndefinedOperatorFunction(operatorName, parametersParseResult.Value!, jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }
}