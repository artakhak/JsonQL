using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Defines a parser interface that converts parsed expressions and context information
/// into an instance of a JSON-based function implementation.
/// </summary>
public interface IJsonFunctionFromExpressionParser
{
    /// <summary>
    /// Parses a JSON function from the provided simple value and expression item within the specified context.
    /// </summary>
    /// <param name="parsedSimpleValue">The parsed simple value representing input data to be processed into a JSON function.</param>
    /// <param name="expressionItem">The expression item providing structural information for parsing the JSON function.</param>
    /// <param name="jsonFunctionContext">The context containing additional information for evaluating or processing the JSON function.</param>
    /// <returns>A parse result containing the parsed JSON function.</returns>
    IParseResult<IJsonFunction> Parse(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase expressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

/// <inheritdoc />
public class JsonFunctionFromExpressionParser : IJsonFunctionFromExpressionParser
{
    private readonly IJsonValuePathJsonFunctionParser _jsonValuePathJsonFunctionParser;
    private readonly INumericValueJsonFunctionFactory _numericValueJsonFunctionFactory;
    private readonly ISpecialLiteralJsonFunctionFactory _specialLiteralJsonFunctionFactory;
    private readonly IConstantTextJsonFunctionFactory _constantTextJsonFunctionFactory;
    private readonly IBracesJsonFunctionFactory _bracesJsonFunctionFactory;
    private readonly IOperatorJsonFunctionFactory _operatorJsonFunctionFactory;

    /// <summary>
    /// Parses expressions into corresponding JSON functions based on provided factories and parsers.
    /// </summary>
    /// <param name="jsonValuePathJsonFunctionParser">The parser responsible for handling JSON value path expressions.</param>
    /// <param name="numericValueJsonFunctionFactory">Factory for creating JSON functions that handle numeric values.</param>
    /// <param name="specialLiteralJsonFunctionFactory">Factory for creating JSON functions for special literal values.</param>
    /// <param name="constantTextJsonFunctionFactory">Factory for creating JSON functions based on constant text values.</param>
    /// <param name="bracesJsonFunctionFactory">Factory for handling JSON functions involving braces or grouped expressions.</param>
    /// <param name="operatorJsonFunctionFactory">Factory for creating JSON functions based on operators within the expression.</param>
    public JsonFunctionFromExpressionParser(IJsonValuePathJsonFunctionParser jsonValuePathJsonFunctionParser,
        INumericValueJsonFunctionFactory numericValueJsonFunctionFactory,
        ISpecialLiteralJsonFunctionFactory specialLiteralJsonFunctionFactory,
        IConstantTextJsonFunctionFactory constantTextJsonFunctionFactory,
        IBracesJsonFunctionFactory bracesJsonFunctionFactory,
        IOperatorJsonFunctionFactory operatorJsonFunctionFactory)
    {
        _jsonValuePathJsonFunctionParser = jsonValuePathJsonFunctionParser;
        _numericValueJsonFunctionFactory = numericValueJsonFunctionFactory;
        _specialLiteralJsonFunctionFactory = specialLiteralJsonFunctionFactory;
        _constantTextJsonFunctionFactory = constantTextJsonFunctionFactory;
        _bracesJsonFunctionFactory = bracesJsonFunctionFactory;
        _operatorJsonFunctionFactory = operatorJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction> Parse(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase expressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        List<IJsonObjectParseError> errors = new List<IJsonObjectParseError>();
      
        var parsedJsonFunction = TryParse(parsedSimpleValue, expressionItem, jsonFunctionContext, errors);

        if (errors.Count > 0)
            return new ParseResult<IJsonFunction>(errors);

        if (parsedJsonFunction == null)
        {
            return new ParseResult<IJsonFunction>(CollectionExpressionHelpers.Create(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol, parsedSimpleValue.LineInfo)));
        }

        return new ParseResult<IJsonFunction>(parsedJsonFunction);
    }

    private IJsonFunction? TryParse(IParsedSimpleValue parsedSimpleValue,
        IExpressionItemBase expressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext,
        List<IJsonObjectParseError> errors)
    {
        // Numeric values: 1.5, 15, etc.
        if (expressionItem is INumericExpressionItem numericExpressionItem)
        {
            var numericValueParseResult = _numericValueJsonFunctionFactory.TryCreateNumericValueFunction(parsedSimpleValue, numericExpressionItem, jsonFunctionContext);

            if (numericValueParseResult.Errors.Count > 0)
            {
                errors.AddRange(numericValueParseResult.Errors);
                return null;
            }

            return numericValueParseResult.Value;
        }

        // Constant texts: 'Some text', 'Another text'
        if (expressionItem is IConstantTextExpressionItem constantTextExpressionItem)
        {
            var numericValueParseResult = _constantTextJsonFunctionFactory.TryCreateConstantTextFunction(parsedSimpleValue, constantTextExpressionItem, jsonFunctionContext);

            if (numericValueParseResult.Errors.Count > 0)
            {
                errors.AddRange(numericValueParseResult.Errors);
                return null;
            }

            return numericValueParseResult.Value;
        }

        // true, false, value, etc.
        if (expressionItem is ILiteralExpressionItem literalExpressionItem)
        {
            var numericValueParseResult = _specialLiteralJsonFunctionFactory.TryCreateSpecialLiteralFunction(parsedSimpleValue, literalExpressionItem, jsonFunctionContext);

            if (numericValueParseResult.Errors.Count > 0)
            {
                errors.AddRange(numericValueParseResult.Errors);
                return null;
            }

            // Not all literal expressions will be parsed. Most literals will be parsed to path elements in code that follows.
            if (numericValueParseResult.Value != null)
                return numericValueParseResult.Value;
        }

        // Allow attribute names that are numbers and be used in json path. We can achieve this by using 'key' path element function
        // Example how it can be used in expression: where(parent.Examples.key(1).Array1[2] > 10)
        var jsonValuePathParseResult = _jsonValuePathJsonFunctionParser.TryParse(parsedSimpleValue, expressionItem, jsonFunctionContext);

        if (jsonValuePathParseResult.Errors.Count > 0)
        {
            errors.AddRange(jsonValuePathParseResult.Errors);
            return null;
        };

        if (jsonValuePathParseResult.Value != null)
            return jsonValuePathParseResult.Value;

        if (expressionItem is IBracesExpressionItem bracesExpressionItem)
        {
            // It is possible the braces expression has no function and is used to change the order of operations like this ((x+y)*z) or (((x+y))*z) 
            // In this case, we want to get the innermost expression
            if (bracesExpressionItem.NameLiteral == null && bracesExpressionItem.OpeningBrace.IsRoundBrace)
            {
                IBracesExpressionItem? currentBracesExpressionItem = bracesExpressionItem;

                while (true)
                {
                    if (currentBracesExpressionItem.Parameters.Count != 1 || currentBracesExpressionItem.Parameters[0] == null)
                    {
                        errors.Add(new JsonObjectParseError("Non-function round braces should have exactly one parameter. Valid example '1+(x+y)*z'. Invalid examples: '1+()*z' or '1+(x, y)'.",
                            parsedSimpleValue.LineInfo.GenerateRelativePosition(currentBracesExpressionItem.OpeningBrace)));
                        return null;
                    }

                    var bracesParameter = currentBracesExpressionItem.Parameters[0];
                    if (bracesParameter == null)
                    {
                        var errorMessage = $"The values in [{nameof(IBracesExpressionItem.Parameters)}] cannot be null if expression parsed.";

                        var lineInfo = parsedSimpleValue.LineInfo.GenerateRelativePosition(currentBracesExpressionItem);

                        ThreadStaticLoggingContext.Context.ErrorFormat("{0} Line info: {1}", errorMessage, lineInfo);

                        errors.Add(new JsonObjectParseError(errorMessage, lineInfo));
                        return null;
                    }

                    currentBracesExpressionItem = bracesParameter as IBracesExpressionItem;

                    if (currentBracesExpressionItem == null || currentBracesExpressionItem.NameLiteral != null || !currentBracesExpressionItem.OpeningBrace.IsRoundBrace)
                        return TryParse(parsedSimpleValue, bracesParameter, jsonFunctionContext, errors);
                }
            }

            if (bracesExpressionItem.NameLiteral != null || !bracesExpressionItem.OpeningBrace.IsRoundBrace)
            {
                var bracesCustomFunctionResult = _bracesJsonFunctionFactory.TryCreateBracesCustomFunction(parsedSimpleValue, bracesExpressionItem, jsonFunctionContext);

                if (bracesCustomFunctionResult.Errors.Count > 0)
                {
                    errors.AddRange(bracesCustomFunctionResult.Errors);
                    return null;
                }
                
                return bracesCustomFunctionResult.Value;
            }
        }

        if (expressionItem is IOperatorExpressionItem operatorExpressionItem)
        {
            var operatorFunctionResult = _operatorJsonFunctionFactory.TryCreateOperatorFunction(parsedSimpleValue, operatorExpressionItem, jsonFunctionContext);

            if (operatorFunctionResult.Errors.Count > 0)
            {
                errors.AddRange(operatorFunctionResult.Errors);
                return null;
            }

            return operatorFunctionResult.Value;
        }

        return null;
    }
}