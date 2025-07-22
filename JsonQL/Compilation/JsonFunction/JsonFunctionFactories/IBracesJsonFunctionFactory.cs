// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;
using JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for parsing <see cref="IBracesExpressionItem"/> (e.g., <b>Lower('EXAMPLE TEXT')</b>) into a <see cref="IJsonFunction"/>.
/// </summary>
public interface IBracesJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from braces expression. 
    /// An example of custom functions is <b>Lower('EXAMPLE TEXT')</b>
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed JSON value which contains the expression to be parsed.</param>
    /// <param name="bracesExpressionItem">Braces expression to convert to <see cref="IJsonFunction"/>.</param>
    /// <param name="jsonFunctionContext">Context.</param>
    /// <returns>
    /// Returns parse result.
    /// If the value <see cref="IParseResult{TValue}.Errors"/> is not empty, function failed to be parsed.<br/>
    /// Otherwise, the value <see cref="IParseResult{TValue}.Value"/> will be non-null, if function parsed, or null, if <see cref="IJsonFunction"/> does not<br/>
    /// know how to parse the expression into <see cref="IJsonFunction"/> (in which case the caller of this method will either try to parse the expression<br/>
    /// some other way, or will report an error).
    /// </returns>
    IParseResult<IJsonFunction> TryCreateBracesCustomFunction(IParsedSimpleValue parsedSimpleValue, IBracesExpressionItem bracesExpressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

/// <summary>
/// A factory for parsing <see cref="IBracesExpressionItem"/> (e.g., <b>Lower('EXAMPLE TEXT')</b>) into a <see cref="IJsonFunction"/>.
/// </summary>
public class BracesJsonFunctionFactory : JsonFunctionFactoryAbstr, IBracesJsonFunctionFactory
{
    /// <inheritdoc />
    public IParseResult<IJsonFunction> TryCreateBracesCustomFunction(IParsedSimpleValue parsedSimpleValue,
        IBracesExpressionItem bracesExpressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        var functionNameLiteralExpression = bracesExpressionItem.NameLiteral;

        if (functionNameLiteralExpression == null)
            return new ParseResult<IJsonFunction>(CollectionExpressionHelpers.Create(
                new JsonObjectParseError("Function name is missing",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(bracesExpressionItem))
            ));

        var functionName = functionNameLiteralExpression.LiteralName.Text;

        var functionParameters = bracesExpressionItem.Parameters;
        var functionLineInfo = parsedSimpleValue.LineInfo.GenerateRelativePosition(functionNameLiteralExpression);
       
        switch (functionName)
        {
            case JsonFunctionNames.CountAggregateLambdaExpressionFunction:
                return CreateCollectionItemsAggregateLambdaExpressionFunction(parsedSimpleValue, functionName,
                    (pathFunction, lambdaPredicate, _) => new CountAggregateLambdaExpressionFunction(functionName, pathFunction, lambdaPredicate, jsonFunctionContext, functionLineInfo),
                    functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.AverageAggregateLambdaExpressionFunction:
                return CreateCollectionItemsAggregateLambdaExpressionFunction(parsedSimpleValue, functionName,
                    (pathFunction, lambdaPredicate, numericValueLambdaFunction) => new AverageAggregateLambdaExpressionFunction(functionName, pathFunction, lambdaPredicate, numericValueLambdaFunction, jsonFunctionContext, functionLineInfo),
                    functionParameters, jsonFunctionContext, functionLineInfo, false, true);

            case JsonFunctionNames.MinAggregateLambdaExpressionFunction:
                return CreateCollectionItemsAggregateLambdaExpressionFunction(parsedSimpleValue, functionName,
                    (pathFunction, lambdaPredicate, numericValueLambdaFunction) => new MinMaxAggregateLambdaExpressionFunction(functionName, true, pathFunction, lambdaPredicate, numericValueLambdaFunction, jsonFunctionContext, functionLineInfo),
                    functionParameters, jsonFunctionContext, functionLineInfo, false, true);

            case JsonFunctionNames.MaxAggregateLambdaExpressionFunction:
                return CreateCollectionItemsAggregateLambdaExpressionFunction(parsedSimpleValue, functionName,
                    (pathFunction, lambdaPredicate, numericValueLambdaFunction) => new MinMaxAggregateLambdaExpressionFunction(functionName, false, pathFunction, lambdaPredicate, numericValueLambdaFunction, jsonFunctionContext, functionLineInfo),
                    functionParameters, jsonFunctionContext, functionLineInfo, false, true);

            case JsonFunctionNames.SumAggregateLambdaExpressionFunction:
                return CreateCollectionItemsAggregateLambdaExpressionFunction(parsedSimpleValue, functionName,
                    (pathFunction, lambdaPredicate, numericValueLambdaFunction) => new SumAggregateLambdaExpressionFunction(functionName, pathFunction, lambdaPredicate, numericValueLambdaFunction, jsonFunctionContext, functionLineInfo),
                    functionParameters, jsonFunctionContext, functionLineInfo, false, true);

            case JsonFunctionNames.AllAggregateLambdaExpressionFunction:
                return CreateCollectionItemsAggregateLambdaExpressionFunction(parsedSimpleValue, functionName,
                    (pathFunction, lambdaPredicate, _) => new AllAggregateLambdaExpressionFunction(functionName, pathFunction, 
                        lambdaPredicate ?? throw new InvalidOperationException("The criteria cannot be null."), 
                        jsonFunctionContext, functionLineInfo),
                    functionParameters, jsonFunctionContext, functionLineInfo, true);

            case JsonFunctionNames.AnyAggregateLambdaExpressionFunction:
                return CreateCollectionItemsAggregateLambdaExpressionFunction(parsedSimpleValue, functionName,
                    (pathFunction, lambdaPredicate, _) => new AnyAggregateLambdaExpressionFunction(functionName, pathFunction, lambdaPredicate, jsonFunctionContext, functionLineInfo),
                    functionParameters, jsonFunctionContext, functionLineInfo);
          
            case JsonFunctionNames.Concatenate:
                return CreateConcatenateValuesJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.StringToLowerCase:
                return CreateTextToLowerUpperCaseJsonFunction(parsedSimpleValue, functionName, functionParameters, true, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.StringToUpperCase:
                return CreateTextToLowerUpperCaseJsonFunction(parsedSimpleValue, functionName, functionParameters, false, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.TextLength:
                return CreateTextLengthJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.ConvertToDateTime:
                return CreateConvertToDateTimeJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.ConvertToDate:
                return CreateConvertToDateJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.ConvertToDouble:
                return CreateConvertToDoubleJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.ConvertToInt:
                return CreateConvertToIntJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.ConvertToBoolean:
                return CreateConvertToBooleanJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.ConvertToString:
                return CreateConvertToStringJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.HasField:
                return CreateJsonObjectHasFieldJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.Abs:
                return CreateAbsoluteValueJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.IsEven:
                return CreateIsEvenValueJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);

            case JsonFunctionNames.IsOdd:
                return CreateIsOddValueJsonFunction(parsedSimpleValue, functionName, functionParameters, jsonFunctionContext, functionLineInfo);
        }

        return new ParseResult<IJsonFunction>(CollectionExpressionHelpers.Create(
            new JsonObjectParseError($"Unknown function [{functionName}]",
                parsedSimpleValue.LineInfo.GenerateRelativePosition(functionNameLiteralExpression))
        ));
    }

    private IParseResult<IJsonFunction> CreateCollectionItemsAggregateLambdaExpressionFunction(IParsedSimpleValue parsedSimpleValue, 
        string functionName,
        Func<IJsonValuePathJsonFunction, IUniversalLambdaFunction?, IUniversalLambdaFunction?, IJsonFunction> createAggregateFunction,
        IReadOnlyList<IExpressionItemBase> functionParameters, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, 
        IJsonLineInfo? functionLineInfo,
        bool criteriaIsRequired = false, bool canHaveNumericValueSelector = false)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonValuePathJsonFunction,
            ILambdaExpressionFunction, ILambdaExpressionFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("collection", typeof(IJsonValuePathJsonFunction), true)
            {
                ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("criteria", typeof(ILambdaExpressionFunction), criteriaIsRequired),
            new JsonFunctionParameterMetadata("value", typeof(ILambdaExpressionFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        IUniversalLambdaFunction? aggregateFunctionPredicate = null;
        IUniversalLambdaFunction? numericValueLambdaFunction = null;

        ParseResult<IJsonFunction> GetLambdaExpressionParametersNumberInvalidError(ILambdaExpressionFunction lambdaExpressionFunction)
        {
            return new ParseResult<IJsonFunction>(
                    CollectionExpressionHelpers.Create(
                new JsonObjectParseError(
                    $"Lambda expression parameter in aggregation function [{functionName}] is expected to have one parameter.",
                    lambdaExpressionFunction.Parameters.Count == 0 ?
                        lambdaExpressionFunction.LineInfo : lambdaExpressionFunction.Parameters[1].LineInfo)
            ));
        }

        if (parametersParseResult.Value.parameter2 != null)
        {
            var lambdaExpressionFunction = parametersParseResult.Value.parameter2;

            if (lambdaExpressionFunction.Parameters.Count != 1)
                return GetLambdaExpressionParametersNumberInvalidError(lambdaExpressionFunction);
        
            aggregateFunctionPredicate = new UniversalLambdaFunction(
                lambdaExpressionFunction.Parameters[0], lambdaExpressionFunction.Expression);
        }

        if (parametersParseResult.Value.parameter3 != null)
        {
            var lambdaExpressionFunction = parametersParseResult.Value.parameter3;

            if (!canHaveNumericValueSelector)
            {
                return new ParseResult<IJsonFunction>(
                        CollectionExpressionHelpers.Create(
                    new JsonObjectParseError(
                        $"Invalid parameter in aggregation function [{functionName}].",
                        parametersParseResult.Value.parameter3.LineInfo)
                ));
            }

            if (lambdaExpressionFunction.Parameters.Count != 1)
                return GetLambdaExpressionParametersNumberInvalidError(lambdaExpressionFunction);

            numericValueLambdaFunction = new UniversalLambdaFunction(
                lambdaExpressionFunction.Parameters[0], lambdaExpressionFunction.Expression);
        }

        parametersJsonFunctionContext.ParentJsonFunction =
            createAggregateFunction(parametersParseResult.Value.parameter1!, aggregateFunctionPredicate, numericValueLambdaFunction);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateConcatenateValuesJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);
        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionCollectionParameter<IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("values", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 || parametersParseResult.Value == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ConcatenateValuesJsonFunction(functionName, parametersParseResult.Value, jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateTextToLowerUpperCaseJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, bool isToLowerCaseFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);
        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 || parametersParseResult.Value == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = isToLowerCaseFunction ?
            new TextToLowerCaseJsonFunction(functionName, parametersParseResult.Value, jsonFunctionContext, functionLineInfo) :
            new TextToUpperCaseJsonFunction(functionName, parametersParseResult.Value, jsonFunctionContext, functionLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateTextLengthJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);
        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("text", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 || parametersParseResult.Value == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction =
            new TextLengthJsonFunction(functionName, parametersParseResult.Value, jsonFunctionContext, functionLineInfo);

        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateConvertToDateTimeJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                // TODO: Check if this should be false
                //ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("assert", typeof(IBooleanJsonFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 ||
            parametersParseResult.Value.parameter1 == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ConvertToDateTimeJsonFunction(functionName,
            parametersParseResult.Value.parameter1, parametersParseResult.Value.parameter2,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateConvertToDateJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                // TODO: Check if this should be false
                //ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("assert", typeof(IBooleanJsonFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 ||
            parametersParseResult.Value.parameter1 == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ConvertToDateJsonFunction(functionName,
            parametersParseResult.Value.parameter1, parametersParseResult.Value.parameter2,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateConvertToDoubleJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                // TODO: Check if this should be false
                //ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("assert", typeof(IBooleanJsonFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 ||
            parametersParseResult.Value.parameter1 == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ConvertToDoubleJsonFunction(functionName,
            parametersParseResult.Value.parameter1, parametersParseResult.Value.parameter2,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateConvertToIntJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                // TODO: Check if this should be false
                //ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("assert", typeof(IBooleanJsonFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 ||
            parametersParseResult.Value.parameter1 == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ConvertToIntJsonFunction(functionName,
            parametersParseResult.Value.parameter1, parametersParseResult.Value.parameter2,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateConvertToBooleanJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                // TODO: Check if this should be false
                //ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("assert", typeof(IBooleanJsonFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 ||
            parametersParseResult.Value.parameter1 == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ConvertToBooleanJsonFunction(functionName, 
            parametersParseResult.Value.parameter1, parametersParseResult.Value.parameter2,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateConvertToStringJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonFunction, IBooleanJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true)
            {
                // TODO: Check if this should be false
                //ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("assert", typeof(IBooleanJsonFunction), false),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 ||
            parametersParseResult.Value.parameter1 == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new ConvertToStringJsonFunction(functionName,
            parametersParseResult.Value.parameter1, parametersParseResult.Value.parameter2,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateJsonObjectHasFieldJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parametersParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameters<IJsonValuePathJsonFunction, IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("path", typeof(IJsonValuePathJsonFunction), true)
            {
                // TODO: Check if this should be false
                //ValidateIsNotMultipleValuesSelectorPath = false
            },
            new JsonFunctionParameterMetadata("key", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parametersParseResult.Errors.Count > 0 || 
            parametersParseResult.Value.parameter1 == null || parametersParseResult.Value.parameter2 == null)
            return new ParseResult<IJsonFunction>(parametersParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new JsonObjectHasFieldJsonFunction(functionName, 
            parametersParseResult.Value.parameter1, parametersParseResult.Value.parameter2, jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateAbsoluteValueJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parameterParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parameterParseResult.Errors.Count > 0 ||
            parameterParseResult.Value == null)
            return new ParseResult<IJsonFunction>(parameterParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new AbsoluteValueJsonFunction(functionName,
            parameterParseResult.Value,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateIsEvenValueJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parameterParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parameterParseResult.Errors.Count > 0 ||
            parameterParseResult.Value == null)
            return new ParseResult<IJsonFunction>(parameterParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new IsEvenValueJsonFunction(functionName,
            parameterParseResult.Value,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }

    private IParseResult<IJsonFunction> CreateIsOddValueJsonFunction(IParsedSimpleValue parsedSimpleValue, string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? functionLineInfo)
    {
        var parametersJsonFunctionContext = new JsonFunctionValueEvaluationContext(jsonFunctionContext.VariablesManager);

        var parameterParseResult = JsonFunctionFromExpressionParser.TryParseJsonFunctionParameter<IJsonFunction>(
            parsedSimpleValue, functionName,
            functionParameters,
            new JsonFunctionParameterMetadata("value", typeof(IJsonFunction), true),
            parametersJsonFunctionContext,
            functionLineInfo);

        if (parameterParseResult.Errors.Count > 0 ||
            parameterParseResult.Value == null)
            return new ParseResult<IJsonFunction>(parameterParseResult.Errors);

        parametersJsonFunctionContext.ParentJsonFunction = new IsOddValueJsonFunction(functionName,
            parameterParseResult.Value,
            jsonFunctionContext, functionLineInfo);
        return new ParseResult<IJsonFunction>(parametersJsonFunctionContext.ParentJsonFunction);
    }
}
