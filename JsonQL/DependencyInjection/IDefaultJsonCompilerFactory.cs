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
    public DefaultJsonCompilerFactory(ILog logger, IStringFormatter stringFormatter)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public IJsonCompiler Create()
    {
        DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory = null;
        IJsonFunctionFromExpressionParser? jsonFunctionFromExpressionParser = null;

        [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
        (bool parameterValueWasResolved, object? resolvedValue) TryResolveConstructorParameterValue(Type type, ParameterInfo parameterInfo)
        {
            if (defaultImplementationBasedObjectFactory == null)
                throw new InvalidOperationException($"The value of [{nameof(defaultImplementationBasedObjectFactory)}] was not set.");

            if (parameterInfo.ParameterType == typeof(IStringFormatter))
                return (true, defaultImplementationBasedObjectFactory.CreateInstance<IDefaultStringFormatterFactory>().Create());
            
            if (parameterInfo.ParameterType == typeof(IJsonFunctionFromExpressionParser))
                return (true, GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));

            if (TryResolveJsonFunctionFactory(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser,
                    parameterInfo.ParameterType, out var jsonFunctionFactory))
                return (true, jsonFunctionFactory);

            return (false, null);
        }

        defaultImplementationBasedObjectFactory = new DefaultImplementationBasedObjectFactory(_ => { },
            TryResolveConstructorParameterValue, type => true, _logger);

        return defaultImplementationBasedObjectFactory.CreateInstance<IJsonCompiler>();
    }

    private static IJsonFunctionFromExpressionParser GetOrCreateJsonFunctionFromExpressionParser(DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory,
        ref IJsonFunctionFromExpressionParser? jsonFunctionFromExpressionParser)
    {
        if (defaultImplementationBasedObjectFactory == null)
            throw new InvalidOperationException($"The value of [{nameof(defaultImplementationBasedObjectFactory)}] was not set.");

        return jsonFunctionFromExpressionParser ??= defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonFunctionFromExpressionParser>();
    }

    private static bool TryResolveJsonFunctionFactory(DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory,
        ref IJsonFunctionFromExpressionParser? jsonFunctionFromExpressionParser,
        Type parameterType, [NotNullWhen(true)] out object? jsonFunctionFactory)
    {
        if (parameterType == typeof(IBracesJsonFunctionFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IBracesJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IBinaryOperatorJsonFunctionFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IBinaryOperatorJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IUnaryPrefixOperatorJsonFunctionFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IUnaryPrefixOperatorJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IUnaryPostfixOperatorJsonFunctionFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IUnaryPostfixOperatorJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IJsonValueCollectionItemsSelectorPathElementFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IJsonValueCollectionItemsSelectorPathElementFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(ISpecialLiteralJsonFunctionFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<ISpecialLiteralJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(INumericValueJsonFunctionFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<INumericValueJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        if (parameterType == typeof(IConstantTextJsonFunctionFactory))
        {
            jsonFunctionFactory = PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                defaultImplementationBasedObjectFactory.CreateInstance<IConstantTextJsonFunctionFactory>(),
                GetOrCreateJsonFunctionFromExpressionParser(defaultImplementationBasedObjectFactory, ref jsonFunctionFromExpressionParser));
            return true;
        }

        jsonFunctionFactory = null;
        return false;
    }
}