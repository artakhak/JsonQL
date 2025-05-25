using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JsonQL.Compilation;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonValueTextGenerator;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;

namespace JsonQL.DependencyInjection;

/// <summary>
/// A factory for creating an instance of <see cref="IJsonCompiler"/> that can be used in most cases when no
/// customization is necessary (out of the box <see cref="IJsonCompiler"/>). 
/// For cases when more customization is necessary, create an instance of an implementation of <see cref="IJsonCompiler"/> normally
/// using the default implementation <see cref="JsonCompiler"/>, and pass the dependencies necessary to constructor.
/// Example of custom setup of <see cref="IJsonCompiler"/> can be found in JsonQL.Demos project
/// in class JsonQL.Demos.Startup.DependencyInjection.JsonQLClassRegistrationsModule
/// </summary>
public interface IDefaultJsonCompilerFactory
{
    /// <summary>
    /// Creates an instance of <see cref="IJsonCompiler"/>.
    /// </summary>
    /// <returns></returns>
    IJsonCompiler Create();
}

/// <inheritdoc />
public class DefaultJsonCompilerFactory : IDefaultJsonCompilerFactory
{
    private readonly ILog _logger;

    /// <summary>
    /// Provides a default implementation for creating instances of <see cref="IJsonCompiler"/>.
    /// </summary>
    public DefaultJsonCompilerFactory(ILog logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public IJsonCompiler Create()
    {
        DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory = null;

        [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
        (bool parameterValueWasResolved, object? resolvedValue) CustomDiResolver(Type type, ParameterInfo parameterInfo)
        {
            if (defaultImplementationBasedObjectFactory == null)
                throw new InvalidOperationException($"The value of [{nameof(defaultImplementationBasedObjectFactory)}] was not set.");

            if (parameterInfo.ParameterType == typeof(ILog))
                return (true, _logger);

            if (parameterInfo.ParameterType == typeof(IStringFormatter))
                return (true, defaultImplementationBasedObjectFactory.CreateInstance<IDefaultStringFormatterFactory>().Create());

            return (false, null);
        }

        defaultImplementationBasedObjectFactory = new DefaultImplementationBasedObjectFactory(_ => { },
            CustomDiResolver, type => true, _logger);

        var jsonCompiler = defaultImplementationBasedObjectFactory.CreateInstance<IJsonCompiler>();
        InitFunctionFactories(defaultImplementationBasedObjectFactory);
        return jsonCompiler;
    }

    private void InitFunctionFactories(DefaultImplementationBasedObjectFactory defaultImplementationBasedObjectFactory)
    {
        var jsonFunctionFromExpressionParser = defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonFunctionFromExpressionParser>();

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBracesJsonFunctionFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<IOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBinaryOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPrefixOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPostfixOperatorJsonFunctionFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonValueCollectionItemsSelectorPathElementFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<ISpecialLiteralJsonFunctionFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<INumericValueJsonFunctionFactory>(), jsonFunctionFromExpressionParser);

        PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
            defaultImplementationBasedObjectFactory.GetOrCreateInstance<IConstantTextJsonFunctionFactory>(), jsonFunctionFromExpressionParser);
    }
}