using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for parsing binary operator expressions (e.g., <b>Object1.Int1 + 10")</b>) into a <see cref="IJsonFunction"/>.
/// </summary>
public interface IBinaryOperatorJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from binary operator expression. 
    /// Example:[x &lt; 10] 
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed json value which contains the expression to be parsed.</param>
    /// <param name="operatorExpressionItem">Operator expression to convert to <see cref="IJsonFunction"/>.</param>
    /// <param name="operand2">Operand 2.</param>
    /// <param name="jsonFunctionContext">If not null, parent function data.</param>
    /// <param name="operand1">Operand 1.</param>
    /// <param name="operatorLineInfo">Operator line info.</param>
    /// <returns>
    /// Returns parse result.
    /// If the value <see cref="IParseResult{TValue}.Errors"/> is not empty, function failed to be parsed.
    /// Otherwise, the value <see cref="IParseResult{TValue}.Value"/> will be non-null, if function parsed, or null, if <see cref="IJsonFunction"/> does not
    /// know how to parse the expression into <see cref="IJsonFunction"/> (in which case the caller of this method will either try to parse the expression
    /// some other way, or will report an error).
    /// </returns>
    IParseResult<IJsonFunction> GetBinaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem,
        IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo);
}

/// <summary>
/// A factory for parsing binary operator expressions (e.g., <b>Object1.Int1 + 10")</b>) into a <see cref="IJsonFunction"/>.
/// </summary>
public class BinaryOperatorJsonFunctionFactory : JsonFunctionFactoryAbstr, IBinaryOperatorJsonFunctionFactory
{
    private static readonly string StartsWithOperatorName = Helpers.GetOperatorName(JsonOperatorNames.StartsWith);
    private static readonly string EndsWithOperatorName = Helpers.GetOperatorName(JsonOperatorNames.EndsWith);

    /// <inheritdoc />
    public IParseResult<IJsonFunction> GetBinaryOperatorFunction(IParsedSimpleValue parsedSimpleValue, IOperatorExpressionItem operatorExpressionItem, IExpressionItemBase operand1, IExpressionItemBase operand2, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        string operatorName = operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name;

        if (operatorName == StartsWithOperatorName)
            return CreateStartsWithOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);

        if (operatorName == EndsWithOperatorName)
            return CreateEndsWithOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);

        switch (operatorName)
        {
            case JsonOperatorNames.EqualsOperator:
                return CreateBinaryEqualityComparisonOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, true,
                    jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.NotEqualsOperator:
                return CreateBinaryEqualityComparisonOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, false,
                    jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.LessThanOperator:
                return CreateBinaryNonEqualityComparisonOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2,
                    BinaryNonEqualityComparisonOperatorType.LessThan, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.LessThanOrEqualOperator:
                return CreateBinaryNonEqualityComparisonOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2,
                    BinaryNonEqualityComparisonOperatorType.LessThanOrEqual, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.GreaterThanOperator:
                return CreateBinaryNonEqualityComparisonOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2,
                    BinaryNonEqualityComparisonOperatorType.GreaterThan, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.GreaterThanOrEqualOperator:
                return CreateBinaryNonEqualityComparisonOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2,
                    BinaryNonEqualityComparisonOperatorType.GreaterThanOrEqual, jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.AddOperator:
                return CreateAddValuesArithmeticOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.SubtractOperator:
                return CreateSubtractValuesArithmeticOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.MultiplyOperator:
                return CreateMultiplyValuesArithmeticOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.DivideOperator:
                return CreateDivideValuesArithmeticOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.QuotientOperator:
                return CreateQuotientArithmeticOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.AndOperator:
                return CreateBinaryAndLogicalOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);
            case JsonOperatorNames.OrOperator:
                return CreateBinaryOrLogicalOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.DefaultValueOperator:
                return CreateDefaultValueOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.ContainsOperator:
                return CreateContainsOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);

            case JsonOperatorNames.LambdaOperator:
                return CreateLambdaOperatorFunction(parsedSimpleValue, operatorName, operand1, operand2, jsonFunctionContext, operatorLineInfo);
        }

        return OperatorJsonFunctionFactoryHelpers.GetGenericErrorResult(parsedSimpleValue, operatorExpressionItem);
    }

    private IParseResult<IJsonFunction> CreateBinaryEqualityComparisonOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2, bool isEqualsOperator,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);
        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = isEqualsOperator ?
            new BinaryEqualsOperatorFunction(
                isEqualsOperator ? JsonOperatorNames.EqualsOperator : JsonOperatorNames.NotEqualsOperator,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
                jsonFunctionContext, operatorLineInfo):
            new BinaryNotEqualsOperatorFunction(
                isEqualsOperator ? JsonOperatorNames.EqualsOperator : JsonOperatorNames.NotEqualsOperator,
                parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
                jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateBinaryNonEqualityComparisonOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        BinaryNonEqualityComparisonOperatorType binaryNonEqualityComparisonOperator,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new BinaryNonEqualityComparisonOperatorFunction(operatorName, binaryNonEqualityComparisonOperator,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateAddValuesArithmeticOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new AddValuesArithmeticOperatorFunction(
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateSubtractValuesArithmeticOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new SubtractValuesArithmeticOperatorFunction(
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateMultiplyValuesArithmeticOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new MultiplyValuesArithmeticOperatorFunction(
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext,
            operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateDivideValuesArithmeticOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction =
            new DivideValuesArithmeticOperatorFunction(parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
                jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateQuotientArithmeticOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction =
            new QuotientArithmeticOperatorFunction(parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
                jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateBinaryAndLogicalOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IBooleanJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IBooleanJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IBooleanJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new BinaryAndLogicalOperatorFunction(operatorName,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext,
            operatorLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateBinaryOrLogicalOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IBooleanJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IBooleanJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IBooleanJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new BinaryOrLogicalOperatorFunction(operatorName,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateDefaultValueOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("primaryValue", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("defaultValue", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        var operand1JsonFunction = parametersParseResult.Value.parameter1!;
        var operand2JsonFunction = parametersParseResult.Value.parameter2!;

        if (operand1JsonFunction is IBooleanJsonFunction booleanJsonFunction1 &&
            operand2JsonFunction is IBooleanJsonFunction booleanJsonFunction2)
        {
            return new ParseResult<IBooleanJsonFunction>(new DefaultBooleanValueOperatorFunction(operatorName, booleanJsonFunction1, booleanJsonFunction2,
                jsonFunctionContext, operatorLineInfo));
        }

        if (operand1JsonFunction is IDoubleJsonFunction doubleJsonFunction1 &&
            operand2JsonFunction is IDoubleJsonFunction doubleJsonFunction2)
        {
            return new ParseResult<IDoubleJsonFunction>(new DefaultDoubleValueOperatorFunction(operatorName, doubleJsonFunction1, doubleJsonFunction2,
                jsonFunctionContext, operatorLineInfo));
        }

        if (operand1JsonFunction is IDateTimeJsonFunction dateTimeJsonFunction1 &&
            operand2JsonFunction is IDateTimeJsonFunction dateTimeJsonFunction2)
        {
            return new ParseResult<IDateTimeJsonFunction>(new DefaultDateTimeValueOperatorFunction(operatorName, dateTimeJsonFunction1, dateTimeJsonFunction2,
                jsonFunctionContext, operatorLineInfo));
        }

        if (operand1JsonFunction is IStringJsonFunction stringJsonFunction1 &&
            operand2JsonFunction is IStringJsonFunction stringJsonFunction2)
        {
            return new ParseResult<IStringJsonFunction>(new DefaultStringValueOperatorFunction(operatorName, stringJsonFunction1, stringJsonFunction2,
                jsonFunctionContext, operatorLineInfo));
        }

        parametersJsonFunctionContext.ParentJsonFunction = new DefaultValueOperatorFunction(operatorName, operand1JsonFunction, operand2JsonFunction,
            jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateContainsOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ContainsOperatorFunction(operatorName,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext,
            operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateStartsWithOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new StartsWithOperatorFunction(operatorName,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext, operatorLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateEndsWithOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);
        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IJsonFunction>(
            parsedSimpleValue, operatorName,
            CollectionExpressionHelpers.Create(operand1, operand2),
            new JsonFunctionParameterMetadata("operand1", typeof(IJsonFunction), true),
            new JsonFunctionParameterMetadata("operand2", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            operatorLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new EndsWithOperatorFunction(operatorName,
            parametersParseResult.Value.parameter1!, parametersParseResult.Value.parameter2!,
            jsonFunctionContext, operatorLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateLambdaOperatorFunction(IParsedSimpleValue parsedSimpleValue,
        string operatorName, IExpressionItemBase operand1, IExpressionItemBase operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? operatorLineInfo)
    {
        // For now we support one parameter lambda expression used in criteria in aggregated expressions like
        // Max(Object1.Array1.where(value > 10), x => x % 2 = 0)
        // In the future we might support multi parameter lambda expressions.
        if (operand1 is not ILiteralExpressionItem literalExpressionItem)
        {
            return new ParseResult<IJsonFunction>(
                CollectionExpressionHelpers.Create(new JsonObjectParseError($"Expected a parameter name in lambda expression [{operatorName}].",
                parsedSimpleValue.LineInfo.GenerateRelativePosition(operand1))));
        }

        var lambdaExpression = this.JsonFunctionFromExpressionParser.Parse(parsedSimpleValue, operand2, jsonFunctionContext);

        if (lambdaExpression.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(lambdaExpression.Errors);

        if (lambdaExpression.Value == null)
            return new ParseResult<IJsonFunction>(CollectionExpressionHelpers.Create(
                new JsonObjectParseError("Failed to parse lambda expression", operatorLineInfo)));

        return new ParseResult<IJsonFunction>(
            new LambdaExpressionFunction(operatorName,
                CollectionExpressionHelpers.Create(
                    new LambdaFunctionParameterJsonFunction(literalExpressionItem.LiteralName.Text, jsonFunctionContext,
                        parsedSimpleValue.LineInfo.GenerateRelativePosition(operand1))
                ),
                lambdaExpression.Value, jsonFunctionContext, operatorLineInfo));
    }
}