using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for paring a numeric expression <see cref="INumericExpressionItem"/> (e.g., 1.3, 2, etc.) into a <see cref="IDoubleJsonFunction"/>.
/// </summary>
public interface INumericValueJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from braces expression. 
    /// Example of special literal functions are: value, true, etc. 
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed json value which contains the expression to be parsed.</param>
    /// <param name="numericExpressionItem">Numeric expression to convert to <see cref="IJsonFunction"/>.</param>
    /// <param name="jsonFunctionContext">If not null, parent function data.</param>
    /// <returns>
    /// Returns parse result.
    /// If the value <see cref="IParseResult{TValue}.Errors"/> is not empty, function failed to be parsed.
    /// Otherwise, the value <see cref="IParseResult{TValue}.Value"/> will be non-null, if function parsed, or null, if <see cref="IJsonFunction"/> does not
    /// know how to parse the expression into <see cref="IJsonFunction"/> (in which case the caller of this method will either try to parse the expression
    /// some other way, or will report an error).
    /// </returns>
    IParseResult<IDoubleJsonFunction> TryCreateNumericValueFunction(IParsedSimpleValue parsedSimpleValue, INumericExpressionItem numericExpressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

public class NumericValueJsonFunctionFactory : JsonFunctionFactoryAbstr, INumericValueJsonFunctionFactory
{
    /// <inheritdoc />
    public IParseResult<IDoubleJsonFunction> TryCreateNumericValueFunction(IParsedSimpleValue parsedSimpleValue, INumericExpressionItem numericExpressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        if (double.TryParse(numericExpressionItem.Value.NumericValue, out var parsedDouble))
            return new ParseResult<IDoubleJsonFunction>(
                new ConstantNumberJsonFunction($"ConstantNumber:{parsedDouble}", parsedDouble, jsonFunctionContext, parsedSimpleValue.LineInfo.GenerateRelativePosition(numericExpressionItem)));
        
        return new ParseResult<IDoubleJsonFunction>(
            CollectionExpressionHelpers.Create(
                new JsonObjectParseError($"Failed to parse [{numericExpressionItem.Value.NumericValue}] to [{typeof(double)}].",
                    parsedSimpleValue.LineInfo.GenerateRelativePosition(numericExpressionItem))
            ));
    }
}