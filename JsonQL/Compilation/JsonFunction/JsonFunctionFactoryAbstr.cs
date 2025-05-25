using UniversalExpressionParser.ExpressionItems;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Base class for implementations of factory interfaces that convert expressions <see cref="IExpressionItemBase"/> into <see cref="IJsonFunction"/>.
/// </summary>
public abstract class JsonFunctionFactoryAbstr
{
    private IJsonFunctionFromExpressionParser? _jsonFunctionFromExpressionParser;

    /// <summary>
    /// This value cannot be injected into a constructor because of circular dependencies.
    /// The value is not in interface <see cref="IJsonValuePathJsonFunctionParser"/> and should be set in DI setup.
    /// </summary>
    protected IJsonFunctionFromExpressionParser JsonFunctionFromExpressionParser
    {
        get => _jsonFunctionFromExpressionParser ?? throw new NullReferenceException($"The value of [{nameof(JsonFunctionFromExpressionParser)}] was not set.");
        set
        {
            if (_jsonFunctionFromExpressionParser != null)
                throw new ApplicationException($"The value of [{nameof(JsonFunctionFromExpressionParser)}] can be set only once.");

            _jsonFunctionFromExpressionParser = value;
        }
    }
}