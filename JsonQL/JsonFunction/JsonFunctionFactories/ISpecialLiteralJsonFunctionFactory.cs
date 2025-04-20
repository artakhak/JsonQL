using JsonQL.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A factory for parsing a special literal <see cref="ILiteralExpressionItem"/> (e.g., true, value, etc.) into a <see cref="IJsonFunction"/>.
/// </summary>
public interface ISpecialLiteralJsonFunctionFactory
{
    /// <summary>
    /// Tries to create <see cref="IJsonFunction"/> from braces expression. 
    /// Example of special literal functions are: value, true, etc. 
    /// </summary>
    /// <param name="parsedSimpleValue">Parsed json value which contains the expression to be parsed.</param>
    /// <param name="literalExpressionItem">Literal expression to convert to <see cref="IJsonFunction"/>.</param>
    /// <param name="jsonFunctionContext">If not null, parent function data.</param>
    /// <returns>
    /// Returns parse result.
    /// If the value <see cref="IParseResult{TValue}.Errors"/> is not empty, function failed to be parsed.
    /// Otherwise, the value <see cref="IParseResult{TValue}.Value"/> will be non-null, if function parsed, or null, if <see cref="IJsonFunction"/> does not
    /// know how to parse the expression into <see cref="IJsonFunction"/> (in which case the caller of this method will either try to parse the expression
    /// some other way, or will report an error).
    /// </returns>
    IParseResult<IJsonFunction?> TryCreateSpecialLiteralFunction(IParsedSimpleValue parsedSimpleValue, ILiteralExpressionItem literalExpressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext);
}

public class SpecialLiteralJsonFunctionFactory : JsonFunctionFactoryAbstr, ISpecialLiteralJsonFunctionFactory
{
    /// <inheritdoc />
    public IParseResult<IJsonFunction?> TryCreateSpecialLiteralFunction(IParsedSimpleValue parsedSimpleValue, ILiteralExpressionItem literalExpressionItem,
        IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        var literalText = literalExpressionItem.LiteralName.Text;

        if (literalText == "true")
        {
            return new ParseResult<IJsonFunction?>(new TrueFalseBooleanValueFunction(literalText, true, jsonFunctionContext,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(literalExpressionItem)));
        }

        if (literalText == "false")
        {
            return new ParseResult<IJsonFunction?>(new TrueFalseBooleanValueFunction(literalText, false, jsonFunctionContext,
                parsedSimpleValue.LineInfo.GenerateRelativePosition(literalExpressionItem)));
        }

        //if (literalText == JsonFunctionNames.ContextValue)
        //{
        //    return new ParseResult<IJsonFunction?>(new ContextValueFunction(jsonFunctionContext, parsedSimpleValue.LineInfo.GenerateRelativePosition(literalExpressionItem)));
        //}

        if (literalText == JsonFunctionNames.ContextValueIndex)
        {
            return new ParseResult<IJsonFunction?>(new CollectionItemIndexValueFunction(JsonFunctionNames.ContextValueIndex, jsonFunctionContext, parsedSimpleValue.LineInfo.GenerateRelativePosition(literalExpressionItem)));
        }

        return new ParseResult<IJsonFunction?>((IJsonFunction?)null);
    }
}