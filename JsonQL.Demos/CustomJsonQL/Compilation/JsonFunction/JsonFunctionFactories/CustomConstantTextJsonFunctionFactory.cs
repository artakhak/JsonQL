using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;

/// <summary>
/// A custom factory for paring a constant text expression <see cref="INumericExpressionItem"/> (e.g., 'Some text', etc.) into a <see cref="IDoubleJsonFunction"/>.
/// </summary>
public class CustomConstantTextJsonFunctionFactory: JsonFunctionFactoryAbstr, IConstantTextJsonFunctionFactory
{
    private readonly IConstantTextJsonFunctionFactory _defaultConstantTextJsonFunctionFactory;

    public CustomConstantTextJsonFunctionFactory(IConstantTextJsonFunctionFactory defaultConstantTextJsonFunctionFactory)
    {
        _defaultConstantTextJsonFunctionFactory = defaultConstantTextJsonFunctionFactory;
    }

    /// <inheritdoc />
    public IParseResult<IStringJsonFunction> TryCreateConstantTextFunction(IParsedSimpleValue parsedSimpleValue, IConstantTextExpressionItem constantTextExpressionItem, IJsonFunctionValueEvaluationContext jsonFunctionContext)
    {
        return _defaultConstantTextJsonFunctionFactory.TryCreateConstantTextFunction(parsedSimpleValue, constantTextExpressionItem, jsonFunctionContext);
    }
}