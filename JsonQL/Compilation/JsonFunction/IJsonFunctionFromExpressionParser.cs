using OROptimizer.Diagnostics.Log;
using JsonQL.Compilation;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction;

public interface IJsonFunctionFromExpressionParser
{
    IParseResult<IJsonFunction> Parse(IParsedSimpleValue parsedSimpleValue, IExpressionItemBase expressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

public class JsonFunctionFromExpressionParser : IJsonFunctionFromExpressionParser
{
    private readonly IJsonValuePathJsonFunctionParser _jsonValuePathJsonFunctionParser;
    private readonly INumericValueJsonFunctionFactory _numericValueJsonFunctionFactory;
    private readonly ISpecialLiteralJsonFunctionFactory _specialLiteralJsonFunctionFactory;
    private readonly IConstantTextJsonFunctionFactory _constantTextJsonFunctionFactory;
    private readonly IBracesJsonFunctionFactory _bracesJsonFunctionFactory;
    private readonly IOperatorJsonFunctionFactory _operatorJsonFunctionFactory;

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
            // In this case, we want to get the inner-most expression
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

                        LogHelper.Context.Log.ErrorFormat("{0} Line info: {1}", errorMessage, lineInfo);

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