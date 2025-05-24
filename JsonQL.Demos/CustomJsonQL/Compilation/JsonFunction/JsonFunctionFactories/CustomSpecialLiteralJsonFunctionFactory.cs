using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A custom factory for parsing a special literal <see cref="ILiteralExpressionItem"/> (e.g., true, value, etc.) into a <see cref="IJsonFunction"/>.
/// </summary>
public class CustomSpecialLiteralJsonFunctionFactory : JsonFunctionFactoryAbstr, ISpecialLiteralJsonFunctionFactory
{
    private readonly ISpecialLiteralJsonFunctionFactory _defaultSpecialLiteralJsonFunctionFactory;

    public CustomSpecialLiteralJsonFunctionFactory(ISpecialLiteralJsonFunctionFactory defaultSpecialLiteralJsonFunctionFactory)
    {
        _defaultSpecialLiteralJsonFunctionFactory = defaultSpecialLiteralJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction?> TryCreateSpecialLiteralFunction(IParsedSimpleValue parsedSimpleValue, ILiteralExpressionItem literalExpressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        return _defaultSpecialLiteralJsonFunctionFactory.TryCreateSpecialLiteralFunction(parsedSimpleValue, literalExpressionItem, jsonFunctionContext);
    }
}