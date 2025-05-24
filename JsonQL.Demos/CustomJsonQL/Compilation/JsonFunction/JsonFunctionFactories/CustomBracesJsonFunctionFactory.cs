using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A custom factory for parsing <see cref="IBracesExpressionItem"/> (e.g., <b>Lower('EXAMPLE TEXT')</b>) into a <see cref="IJsonFunction"/>.
/// </summary>
public class CustomBracesJsonFunctionFactory: JsonFunctionFactoryAbstr, IBracesJsonFunctionFactory
{
    private readonly IBracesJsonFunctionFactory _defaultBracesJsonFunctionFactory;

    public CustomBracesJsonFunctionFactory(IBracesJsonFunctionFactory defaultBracesJsonFunctionFactory)
    {
        _defaultBracesJsonFunctionFactory = defaultBracesJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IJsonFunction> TryCreateBracesCustomFunction(IParsedSimpleValue parsedSimpleValue, IBracesExpressionItem bracesExpressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        return _defaultBracesJsonFunctionFactory.TryCreateBracesCustomFunction(parsedSimpleValue, bracesExpressionItem, jsonFunctionContext);
    }
}