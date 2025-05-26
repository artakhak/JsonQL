using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A custom factory for paring a numeric expression <see cref="INumericExpressionItem"/> (e.g., 1.3, 2, etc.) into a <see cref="IDoubleJsonFunction"/>.
/// See <see cref="NumericValueJsonFunctionFactory"/> for examples. 
/// </summary>
public class CustomNumericValueJsonFunctionFactory: JsonFunctionFactoryAbstr, INumericValueJsonFunctionFactory
{
    private readonly INumericValueJsonFunctionFactory _defaultNumericValueJsonFunctionFactory;

    public CustomNumericValueJsonFunctionFactory(INumericValueJsonFunctionFactory defaultNumericValueJsonFunctionFactory)
    {
        _defaultNumericValueJsonFunctionFactory = defaultNumericValueJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IDoubleJsonFunction> TryCreateNumericValueFunction(IParsedSimpleValue parsedSimpleValue, INumericExpressionItem numericExpressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        // TODO: this will rarely need to be customized, since the default implementation of INumericValueJsonFunctionFactory
        // should work in most cases (and if not, we could also specify additional numeric value formats using
        // CustomJsonExpressionLanguageProvider.NumericTypeDescriptors.
        // However, if necessary, provide custom implementation here
        return _defaultNumericValueJsonFunctionFactory.TryCreateNumericValueFunction(parsedSimpleValue, numericExpressionItem, jsonFunctionContext);
    }
}