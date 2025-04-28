using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;
using UniversalExpressionParser;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Provides helper methods for parsing JSON functions from an expression.
/// </summary>
public static class JsonFunctionFromExpressionParseHelpers
{
    /// <summary>
    /// Attempts to parse a single JSON function parameter and returns an <see cref="IParseResult{T}"/> instance.
    /// If the parsing is successful, the returned value will contain the parsed parameter corresponding to the provided <paramref name="functionParameterMetadata"/>.
    /// </summary>
    /// <typeparam name="T">The type of the JSON function to be returned, which must implement <see cref="IJsonFunction"/>.</typeparam>
    /// <param name="parser">The JSON function parser used to perform the operation.</param>
    /// <param name="parsedSimpleValue">The parsed simple value from the JSON input.</param>
    /// <param name="functionName">The name of the function whose parameter is being parsed.</param>
    /// <param name="functionParameters">The list of function parameter expressions for the function being parsed.</param>
    /// <param name="functionParameterMetadata">Metadata describing the expected function parameter to be parsed.</param>
    /// <param name="jsonFunctionContext">The evaluation context containing additional information for parsing or evaluating the JSON function, if available.</param>
    /// <param name="functionLineInfo">The line and position information of the function in the JSON source, if available.</param>
    /// <returns>An <see cref="IParseResult{T}"/> holding the parsed function parameter, or errors if parsing fails.</returns>
    public static IParseResult<T?> TryParseJsonFunctionParameter<T>(this IJsonFunctionFromExpressionParser parser,
        IParsedSimpleValue parsedSimpleValue,
        string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionParameterMetadata functionParameterMetadata,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? functionLineInfo) where T : IJsonFunction
    {
        var parsedJsonFunctionsResult = parser.TryParseJsonFunctionParameters(parsedSimpleValue, functionName,
            functionParameters,
            CollectionExpressionHelpers.Create(functionParameterMetadata), jsonFunctionContext, functionLineInfo);

        if (parsedJsonFunctionsResult.Errors.Count > 0 || parsedJsonFunctionsResult.Value == null || parsedJsonFunctionsResult.Value.Count != 1)
            return new ParseResult<T>(parsedJsonFunctionsResult.Errors);

        return new ParseResult<T?>((T?)parsedJsonFunctionsResult.Value[0]);
    }

    /// <summary>
    /// Attempts to parse two JSON function parameters and returns an <see cref="IParseResult{TValue}"/> containing a tuple with the parsed parameters.
    /// If the parsing is successful, the returned tuple will include the parsed parameters corresponding to the provided metadata.
    /// </summary>
    /// <typeparam name="T1">The type of the first JSON function to be parsed, which must implement <see cref="IJsonFunction"/>.</typeparam>
    /// <typeparam name="T2">The type of the second JSON function to be parsed, which must implement <see cref="IJsonFunction"/>.</typeparam>
    /// <param name="parser">The JSON function parser used to perform the operation.</param>
    /// <param name="parsedSimpleValue">The parsed simple value from the JSON input.</param>
    /// <param name="functionName">The name of the function whose parameters are being parsed.</param>
    /// <param name="functionParameters">The list of function parameter expressions for the function being parsed.</param>
    /// <param name="functionParameterMetadata1">Metadata describing the expected first function parameter to be parsed.</param>
    /// <param name="functionParameterMetadata2">Metadata describing the expected second function parameter to be parsed.</param>
    /// <param name="jsonFunctionContext">The evaluation context containing additional information for parsing or evaluating the JSON function, if available.</param>
    /// <param name="functionLineInfo">The line and position information of the function in the JSON source, if available.</param>
    /// <returns>An <see cref="IParseResult{(T1?, T2?)}"/> holding a tuple with the parsed function parameters, or errors if the parsing fails.</returns>
    public static IParseResult<(T1? parameter1, T2? parameter2)> TryParseJsonFunctionParameters<T1, T2>(this IJsonFunctionFromExpressionParser parser,
        IParsedSimpleValue parsedSimpleValue,
        string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionParameterMetadata functionParameterMetadata1, IJsonFunctionParameterMetadata functionParameterMetadata2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? functionLineInfo) where T1 : IJsonFunction where T2 : IJsonFunction
    {
        var parsedJsonFunctionsResult = parser.TryParseJsonFunctionParameters(parsedSimpleValue, functionName,
            functionParameters, CollectionExpressionHelpers.Create(functionParameterMetadata1, functionParameterMetadata2), 
            jsonFunctionContext, functionLineInfo);

        if (parsedJsonFunctionsResult.Errors.Count > 0 || parsedJsonFunctionsResult.Value == null || parsedJsonFunctionsResult.Value.Count != 2)
            return new ParseResult<(T1?, T2?)>(parsedJsonFunctionsResult.Errors);

        return new ParseResult<(T1? parameter1, T2? parameter2)>(((T1?)parsedJsonFunctionsResult.Value[0], (T2?)parsedJsonFunctionsResult.Value[1]));
    }

    /// <summary>
    /// Attempts to parse multiple JSON function parameters and returns an <see cref="IParseResult{T}"/> instance containing a tuple of parsed parameters.
    /// If the parsing is successful, the tuple will include the parsed parameters corresponding to the provided metadata.
    /// </summary>
    /// <typeparam name="T1">The type of the first JSON function to be returned, which must implement <see cref="IJsonFunction"/>.</typeparam>
    /// <typeparam name="T2">The type of the second JSON function to be returned, which must implement <see cref="IJsonFunction"/>.</typeparam>
    /// <typeparam name="T3">The type of the third JSON function to be returned, which must implement <see cref="IJsonFunction"/>.</typeparam>
    /// <param name="parser">The JSON function parser used to perform the operation.</param>
    /// <param name="parsedSimpleValue">The parsed simple value from the JSON input.</param>
    /// <param name="functionName">The name of the function whose parameters are being parsed.</param>
    /// <param name="functionParameters">The list of function parameter expressions for the function being parsed.</param>
    /// <param name="functionParameterMetadata1">Metadata describing the expected first function parameter to be parsed.</param>
    /// <param name="functionParameterMetadata2">Metadata describing the expected second function parameter to be parsed.</param>
    /// <param name="functionParameterMetadata3">Metadata describing the expected third function parameter to be parsed.</param>
    /// <param name="jsonFunctionContext">The evaluation context containing additional information for parsing or evaluating the JSON function, if available.</param>
    /// <param name="functionLineInfo">The line and position information of the function in the JSON source, if available.</param>
    /// <returns>An <see cref="IParseResult{T}"/> holding a tuple with the parsed function parameters, or errors if parsing fails.</returns>
    public static IParseResult<(T1? parameter1, T2? parameter2, T3? parameter3)> TryParseJsonFunctionParameters<T1, T2, T3>(
        this IJsonFunctionFromExpressionParser parser,
        IParsedSimpleValue parsedSimpleValue,
        string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionParameterMetadata functionParameterMetadata1,
        IJsonFunctionParameterMetadata functionParameterMetadata2,
        IJsonFunctionParameterMetadata functionParameterMetadata3,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? functionLineInfo) where T1 : IJsonFunction where T2 : IJsonFunction where T3 : IJsonFunction
    {
        var parsedJsonFunctionsResult = parser.TryParseJsonFunctionParameters(parsedSimpleValue, functionName,
            functionParameters, CollectionExpressionHelpers.Create(functionParameterMetadata1, functionParameterMetadata2, functionParameterMetadata3),
            jsonFunctionContext, functionLineInfo);

        if (parsedJsonFunctionsResult.Errors.Count > 0 || parsedJsonFunctionsResult.Value == null || parsedJsonFunctionsResult.Value.Count != 3)
            return new ParseResult<(T1?, T2?, T3?)>(parsedJsonFunctionsResult.Errors);

        return new ParseResult<(T1?, T2?, T3?)>(((T1?)parsedJsonFunctionsResult.Value[0], (T2?)parsedJsonFunctionsResult.Value[1], (T3?)parsedJsonFunctionsResult.Value[2]));
    }

    private class ResolvedParameterData
    {
        public IJsonFunctionParameterMetadata ParameterMetadata { get; }
        public IExpressionItemBase ResolvedParameter { get; }
        public bool IsNamedParameter { get; }

        public ResolvedParameterData(IJsonFunctionParameterMetadata parameterMetadata, IExpressionItemBase resolvedParameter, bool isNamedParameter)
        {
            ParameterMetadata = parameterMetadata;
            ResolvedParameter = resolvedParameter;
            IsNamedParameter = isNamedParameter;
        }
    }

    /// <summary>
    /// Returns <see cref="IParseResult{TValue}"/> for parsed parameter functions (i.e., instances of <see cref="IJsonFunction"/>).
    /// If the list <see cref="IParseResult{TValue}.Errors"/> in returned value is empty, the list <see cref="IParseResult{TValue}.Value"/>
    /// will contain number of parameter functions equal to the number of items in <param name="parametersMetadata"></param>, and
    /// each parameter in <see cref="IParseResult{TValue}.Value"/> will be a parsed parameter for a corresponding parameter metadata
    /// in <param name="parametersMetadata"></param>
    /// </summary>
    /// <param name="parser">Parser.</param>
    /// <param name="parsedSimpleValue">Parsed simple value.</param>
    /// <param name="functionName">Function name.</param>
    /// <param name="functionParameters">Function parameter expressions.</param>
    /// <param name="parametersMetadata">Parameters metadata.</param>
    /// <param name="jsonFunctionContext">If not null, parent data.</param>
    /// <param name="functionLineInfo">Function line info.</param>
    public static IParseResult<IReadOnlyList<IJsonFunction?>> TryParseJsonFunctionParameters(this IJsonFunctionFromExpressionParser parser,
        IParsedSimpleValue parsedSimpleValue,
        string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters,
        IReadOnlyList<IJsonFunctionParameterMetadata> parametersMetadata,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? functionLineInfo)
    {

        #region Examples of named parameter cases
        // Example method:void Test(int param1, int? param2)
        // Valid
        // Test(param2:3, param3:10, param1:7);

        // Valid
        // Test(param1:3, param3:10, param2:7);

        // Invalid
        // Test(param2:1, 2, 3);
        // Error: Named argument 'param2' is used out-of-position but is followed by an unnamed argument
        // We should check that named parameters are not followed by unnamed parameters.

        #endregion

        if (functionParameters.Count > parametersMetadata.Count)
        {
            return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                    CollectionExpressionHelpers.Create(
                new JsonObjectParseError($"Too many parameters provided for function [{functionName}]. Expected at most {parametersMetadata.Count} parameters.",
                    functionLineInfo)
            ));
        }

        var parameterNameToParameterMetadataIndex = new Dictionary<string, int>(StringComparer.Ordinal);
        
        for (var parameterMetadataIndex = 0; parameterMetadataIndex < parametersMetadata.Count; ++parameterMetadataIndex)
        {
            var parameterMetadata = parametersMetadata[parameterMetadataIndex];

            if (!parameterNameToParameterMetadataIndex.TryAdd(parameterMetadata.Name, parameterMetadataIndex))
            {
                var errorMessage = $"Parameter name [{parameterMetadata.Name}] is specified multiple times in function metadata for function [{functionName}].";

                ThreadStaticLogging.Log.Error(errorMessage);
                return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                    CollectionExpressionHelpers.Create(new JsonObjectParseError(errorMessage, functionLineInfo)));
            }
        }

        var parameterNameToParameter = new Dictionary<string, IExpressionItemBase>(StringComparer.Ordinal);
        var resolvedParameters = new List<ResolvedParameterData>(parametersMetadata.Count);
       
        var metadataIndexToResolvedParameterData = new Dictionary<int, ResolvedParameterData>();

        for (var parameterIndex = 0; parameterIndex < functionParameters.Count; ++parameterIndex)
        {
            var functionParameterExpression = functionParameters[parameterIndex];

            if (functionParameterExpression is IOperatorExpressionItem operatorExpressionItem &&
                operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.OperatorType == OperatorType.BinaryOperator &&
                operatorExpressionItem.OperatorInfoExpressionItem.OperatorInfo.Name == JsonOperatorNames.JsonFunctionParameterValueOperator &&
                operatorExpressionItem.Operand1 is ILiteralExpressionItem operand1 &&
                operatorExpressionItem.Operand2 is not null)
            {
                var parameterName = operand1.LiteralName.Text;

                if (parameterNameToParameter.ContainsKey(parameterName))
                {
                    return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                            CollectionExpressionHelpers.Create(
                        new JsonObjectParseError($"Named parameter [{operand1.LiteralName.Text}] appears multiple times in function [{functionName}].",
                            parsedSimpleValue.LineInfo.GenerateRelativePosition(operand1))
                    ));
                }
                
                if (!parameterNameToParameterMetadataIndex.TryGetValue(parameterName, out var parameterMetadataIndex))
                    return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                            CollectionExpressionHelpers.Create(
                        new JsonObjectParseError($"Invalid parameter name [{operand1.LiteralName.Text}] in function [{functionName}].",
                            parsedSimpleValue.LineInfo.GenerateRelativePosition(operand1))
                    ));

                if (parameterIndex > parameterMetadataIndex &&
                    metadataIndexToResolvedParameterData.TryGetValue(parameterMetadataIndex, out var resolvedParameterDataAtMetadataIndex) &&
                    !resolvedParameterDataAtMetadataIndex.IsNamedParameter)
                {
                    return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                            CollectionExpressionHelpers.Create(
                        new JsonObjectParseError($"Named parameter [{parameterName}] is used both at position [{parameterIndex + 1}] as well as at position [{parameterMetadataIndex + 1}] as un-named parameter in function [{functionName}].",
                            parsedSimpleValue.LineInfo.GenerateRelativePosition(operand1))
                    ));
                }

                var namedParameter = operatorExpressionItem.Operand2;
                parameterNameToParameter[parameterName] = namedParameter;

                var resolvedParameter = new ResolvedParameterData(parametersMetadata[parameterMetadataIndex], namedParameter, true);

                resolvedParameters.Add(resolvedParameter);
                metadataIndexToResolvedParameterData[parameterMetadataIndex] = resolvedParameter;
            }
            else
            {
                var parameterMetadata = parametersMetadata[parameterIndex];
                var resolvedParameter = new ResolvedParameterData(parameterMetadata, functionParameters[parameterIndex], false);

                resolvedParameters.Add(resolvedParameter);
                metadataIndexToResolvedParameterData[parameterIndex] = resolvedParameter;

                if (parameterIndex > 0)
                {
                    var previousParameter = resolvedParameters[parameterIndex - 1];

                    if (previousParameter.IsNamedParameter)
                    {
                        return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                                CollectionExpressionHelpers.Create(
                            new JsonObjectParseError($"Named parameter [{previousParameter.ParameterMetadata.Name}] is used out-of-position and is followed by an unnamed parameter in function [{functionName}].",
                                parsedSimpleValue.LineInfo.GenerateRelativePosition(previousParameter.ResolvedParameter))
                        ));
                    }
                }
            }
        }

        var resolvedJsonFunctions = new IJsonFunction?[parametersMetadata.Count];

        for (var parameterIndex = 0; parameterIndex < parametersMetadata.Count; ++parameterIndex)
        {
            var parameterMetadata = parametersMetadata[parameterIndex];

            if (!parameterMetadata.IsRequired)
                continue;

            if (!metadataIndexToResolvedParameterData.ContainsKey(parameterIndex))
                return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                        CollectionExpressionHelpers.Create(
                    new JsonObjectParseError($"Parameter [{parameterMetadata.Name}] is required and is missing in function [{functionName}].",
                        functionLineInfo)
                ));
        }

        var errors = new List<IJsonObjectParseError>(3);

        for (var parameterIndex = 0; parameterIndex < resolvedParameters.Count; ++parameterIndex)
        {
            var resolvedParameterData = resolvedParameters[parameterIndex];

            if (!parameterNameToParameterMetadataIndex.TryGetValue(resolvedParameterData.ParameterMetadata.Name, out var parameterMetadataIndex))
            {
                return new ParseResult<IReadOnlyList<IJsonFunction?>>(
                        CollectionExpressionHelpers.Create(
                    new JsonObjectParseError($"Invalid state reached, No data found for parameter [{resolvedParameterData.ParameterMetadata.Name}] in function [{functionName}].",
                        parsedSimpleValue.LineInfo)
                ));
            }

            var parsedFunction = TryParseJsonFunction(parser, functionName, resolvedParameterData.ResolvedParameter, resolvedParameterData.ParameterMetadata,
                parsedSimpleValue, jsonFunctionContext, errors);

            if (errors.Count > 0 || parsedFunction == null)
            {
                return new ParseResult<IReadOnlyList<IJsonFunction?>>(errors);
            }

            resolvedJsonFunctions[parameterMetadataIndex] = parsedFunction;
        }

        return new ParseResult<IReadOnlyList<IJsonFunction?>>(resolvedJsonFunctions);
    }

    /// <summary>
    /// Attempts to parse a collection of JSON function parameters and returns an <see cref="IParseResult{T}"/> containing a list of parsed JSON functions.
    /// If parsing succeeds, the result will contain a collection of <typeparamref name="TJsonFunction"/> instances corresponding to the provided <paramref name="functionParameters"/>.
    /// </summary>
    /// <typeparam name="TJsonFunction">The type of the JSON functions to be parsed, which must implement <see cref="IJsonFunction"/>.</typeparam>
    /// <param name="parser">The JSON function parser responsible for handling the parsing of the function parameters.</param>
    /// <param name="parsedSimpleValue">The parsed simple value representing additional contextual information for the JSON input.</param>
    /// <param name="functionName">The name of the function whose parameters are being parsed.</param>
    /// <param name="functionParameters">The list of function parameter expressions to be parsed into <typeparamref name="TJsonFunction"/> instances.</param>
    /// <param name="parameterMetadata">Metadata describing the type and requirements of the function parameters that need to be parsed.</param>
    /// <param name="jsonFunctionContext">The evaluation context used for parsing or evaluating JSON functions, providing additional contextual data if applicable.</param>
    /// <param name="functionLineInfo">The line and position information from the JSON source that corresponds to the function being parsed, if available.</param>
    /// <returns>An <see cref="IParseResult{T}"/> containing the parsed JSON function collection, or errors in case parsing fails.</returns>
    public static IParseResult<IReadOnlyList<TJsonFunction>> TryParseJsonFunctionCollectionParameter<TJsonFunction>(this IJsonFunctionFromExpressionParser parser,
        IParsedSimpleValue parsedSimpleValue,
        string functionName,
        IReadOnlyList<IExpressionItemBase> functionParameters,
        IJsonFunctionParameterMetadata parameterMetadata,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? functionLineInfo) where TJsonFunction : IJsonFunction
    {
        List<TJsonFunction> parsedFunctions = new List<TJsonFunction>(functionParameters.Count);

        var errors = new List<IJsonObjectParseError>(3);
        foreach (var functionParameter in functionParameters)
        {
            var parsedFunction = TryParseJsonFunction(parser, functionName, functionParameter, parameterMetadata, parsedSimpleValue, jsonFunctionContext, errors);

            if (errors.Count > 0)
            {
                return new ParseResult<IReadOnlyList<TJsonFunction>>(errors);
            }

            if (parsedFunction is not TJsonFunction jsonFunctionOfExpectedType)
            {
                return new ParseResult<IReadOnlyList<TJsonFunction>>(
                    CollectionExpressionHelpers.Create(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(functionParameter))));
            }

            parsedFunctions.Add(jsonFunctionOfExpectedType);
        }

        return new ParseResult<IReadOnlyList<TJsonFunction>>(parsedFunctions);
    }

    private static IJsonFunction? TryParseJsonFunction(IJsonFunctionFromExpressionParser parser,
        string functionName,
        IExpressionItemBase parameter,
        IJsonFunctionParameterMetadata parameterMetadata,
        IParsedSimpleValue parsedSimpleValue,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        List<IJsonObjectParseError> errors)
    {
        var parsedJsonFunctionResult = parser.Parse(parsedSimpleValue, parameter, jsonFunctionContext);

        if (parsedJsonFunctionResult.Errors.Count > 0 || parsedJsonFunctionResult.Value == null)
        {
            if (parsedJsonFunctionResult.Errors.Count == 0)
            {
                errors.Add(new JsonObjectParseError($"Failed to parse a json function parameter from expression for function [{functionName}].",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(parameter)));
            }
            else
            {
                errors.AddRange(parsedJsonFunctionResult.Errors);
            }
            
            return null;
        }

        if (!parameterMetadata.ExpectedParameterFunctionType.IsInstanceOfType(parsedJsonFunctionResult.Value))
        {
            string? typeName = null;

            switch (parameterMetadata.ExpectedParameterFunctionType)
            {
                case IBooleanJsonFunction:
                    typeName = "boolean";
                    break;

                case IDoubleJsonFunction:
                    typeName = "numeric";
                    break;

                case IDateTimeJsonFunction:
                    typeName = "DateTime";
                    break;

                case IStringJsonFunction:
                    typeName = "string";
                    break;
            }

            errors.Add(new JsonObjectParseError(typeName != null ?
                    $"Expected a '{typeName}' value of parameter [{parameterMetadata.Name}] in function [{functionName}]." :
                    $"Invalid value of parameter [{parameterMetadata.Name}] in function [{functionName}].",
                parsedSimpleValue.LineInfo.GenerateRelativePosition(parameter)));

            return null;
        }

        if (!ValidateParameter(functionName, parsedJsonFunctionResult.Value, parameterMetadata, out var validationErrors))
        {
            errors.AddRange(validationErrors);
            return null;
        }

        return parsedJsonFunctionResult.Value;
    }

    private static bool ValidateParameter(string functionName, IJsonFunction parameterJsonFunction, IJsonFunctionParameterMetadata parameterMetadata, [NotNullWhen(false)] out IReadOnlyList<IJsonObjectParseError>? validationErrors)
    {
        validationErrors = null;

        if (parameterMetadata.ValidateIsNotMultipleValuesSelectorPath && parameterJsonFunction is IJsonValuePathJsonFunction jsonValuePathJsonFunction)
        {
            if (jsonValuePathJsonFunction.JsonValuePath.Path.Last() is IJsonValueCollectionItemsSelectorPathElement jsonValueCollectionItemsSelectorPathElement &&
                !jsonValueCollectionItemsSelectorPathElement.SelectsSingleItem)
            {
                validationErrors =
                    CollectionExpressionHelpers.Create(
                    new JsonObjectParseError($"Multiple items selector [{jsonValueCollectionItemsSelectorPathElement.FunctionName}] cannot be used in json path that is a parameter for [{functionName}] function.",
                        jsonValueCollectionItemsSelectorPathElement.LineInfo)
                );
                return false;
            }
        }

        return true;
    }
}