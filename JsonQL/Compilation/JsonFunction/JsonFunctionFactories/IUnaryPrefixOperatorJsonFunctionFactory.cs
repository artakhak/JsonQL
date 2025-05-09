// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// Represents a factory interface for creating JSON functions that handle unary prefix operator expressions.
/// </summary>
/// <remarks>
/// A unary prefix operator is an operator placed before its operand such as "-" or "!". This interface provides
/// the necessary functionality to parse and construct JSON functions for such operations.
/// </remarks>
/// <example>
/// Examples of unary expressions parsed by operators created by this factory are operators "-" and "!" in these expressions:<br/>
/// <b>-Object1.Int1</b><br/>
/// <b>!x[0].Bool1</b>
/// </example>
public interface IUnaryPrefixOperatorJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from unary prefix operator expression. 
    /// Examples:<b>-Object1.Int1</b> or <b>!x[0].Bool1</b>
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed JSON value which contains the expression to be parsed.</param>
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

/// <summary>
/// Represents a factory for creating JSON functions corresponding to unary prefix operator expressions.
/// </summary>
/// <remarks>
/// A unary prefix operator is an operator applied to a single operand, positioned before the operand, such as "-" (negative value operator),
/// "!" (negate boolean value operator), or "typeof" (type evaluation operator). This factory provides methods for constructing JSON functions
/// to process these operators and their respective operands within the context of the parsed JSON data.
/// </remarks>
/// <seealso cref="IUnaryPrefixOperatorJsonFunctionFactory" />
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