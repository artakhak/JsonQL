// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonToObjectConversion;
using JsonQL.JsonToObjectConversion.Serializers;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;

namespace JsonQL.DependencyInjection;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Represents a factory for creating instances of types specific to JsonQL dependency injection.
/// This interface extends <see cref="IDefaultImplementationBasedObjectFactoryEx"/> by inheriting the ability
/// to register, unregister, and manage custom constructor parameter resolvers.
/// </summary>
public interface IJsonQLDefaultImplementationBasedObjectFactory: IDefaultImplementationBasedObjectFactoryEx
{
}

// ReSharper disable once InconsistentNaming
/// <inheritdoc />
public class JsonQLDefaultImplementationBasedObjectFactory : IJsonQLDefaultImplementationBasedObjectFactory
{
    public static readonly Guid DefaultCustomConstructorParameterResolverIdentifier = Guid.Parse("DA7E91B6-99A1-4B38-AED8-6CBCA5C6C070");

    private readonly IDefaultImplementationBasedObjectFactoryEx _defaultImplementationBasedObjectFactory;

    private readonly object _lockObject = new object();

    /// Provides a default implementation for creating objects using the `DefaultImplementationBasedObjectFactory`
    /// with custom constructor parameter resolvers for JSON-QL dependency injection scenarios.
    /// Supports resolving and caching of created object instances and allows for the log diagnostics and object creation event handling.
    public JsonQLDefaultImplementationBasedObjectFactory(Func<Type, bool?>? resolvedTypeInstanceCanBeCached = null, ILog? logger = null) 
    {
        var defaultImplementationBasedObjectFactory = new DefaultImplementationBasedObjectFactory(
            type => resolvedTypeInstanceCanBeCached?.Invoke(type)??true, logger);

        defaultImplementationBasedObjectFactory.RegisterCustomConstructorParameterResolvers(new CustomConstructorParameterResolver(
            DefaultCustomConstructorParameterResolverIdentifier,
            (_, _, parameterInfo) =>
            {
                if (_defaultImplementationBasedObjectFactory == null)
                    throw new InvalidOperationException($"The value of [{nameof(_defaultImplementationBasedObjectFactory)}] was not set.");

                if (parameterInfo.ParameterType == typeof(ILog))
                    return (true, logger);

                if (parameterInfo.ParameterType == typeof(IStringFormatter))
                    return (true, _defaultImplementationBasedObjectFactory.CreateInstance<IDefaultStringFormatterFactory>().Create());

                if (TryResolveSimpleJsonValueSerializer(this, parameterInfo.ParameterType, out var simpleJsonValueSerializer))
                    return (true, simpleJsonValueSerializer);

                if (TryResolveJsonConversionSettings(parameterInfo.ParameterType, out var jsonConversionSettings))
                    return (true, jsonConversionSettings);

                return (false, null);
            }, CustomConstructorParameterResolverPriority.Medium));

        _defaultImplementationBasedObjectFactory = defaultImplementationBasedObjectFactory;
        _defaultImplementationBasedObjectFactory.ResolvedTypeInstanceWasCreated += (sender, e) =>
        {
            this.ResolvedTypeInstanceWasCreated?.Invoke(this, e);
        };
    }

    #region IDefaultImplementationBasedObjectFactory
    /// <inheritdoc />
    public object CreateInstance(Type typeToResolve)
    {
        lock (_lockObject)
        {
            if (typeToResolve == typeof(IJsonCompiler))
            {
                var jsonCompiler = _defaultImplementationBasedObjectFactory.CreateInstance<IJsonCompiler>();
                InitFunctionFactories(_defaultImplementationBasedObjectFactory);
                return jsonCompiler;
            }

            return _defaultImplementationBasedObjectFactory.GetOrCreateInstance(typeToResolve);
        }
    }

    /// <inheritdoc />
    public object GetOrCreateInstance(Type typeToResolve)
    {
        lock (_lockObject)
        {
            return _defaultImplementationBasedObjectFactory.GetOrCreateInstance(typeToResolve);
        }
    }

    public event EventHandler<ResolvedTypeInstanceWasCreated>? ResolvedTypeInstanceWasCreated; 
    #endregion

    #region IDefaultImplementationBasedObjectFactoryEx
    public IEnumerable<ICustomConstructorParameterResolver> GetCustomConstructorParameterResolvers()
    {
        lock (_lockObject)
        {
            return _defaultImplementationBasedObjectFactory.GetCustomConstructorParameterResolvers();
        }
    }

    public void RemoveAllCustomConstructorParameterResolvers()
    {
        lock (_lockObject)
        {
            _defaultImplementationBasedObjectFactory.RemoveAllCustomConstructorParameterResolvers();
        }
    }

    public void RegisterCustomConstructorParameterResolvers(ICustomConstructorParameterResolver customConstructorParameterResolver)
    {
        lock (_lockObject)
        {
            _defaultImplementationBasedObjectFactory.RegisterCustomConstructorParameterResolvers(customConstructorParameterResolver);
        }
    }

    public void UnregisterCustomConstructorParameterResolver(Guid customConstructorParameterResolverIdentifier)
    {
        lock (_lockObject)
        {
            _defaultImplementationBasedObjectFactory.UnregisterCustomConstructorParameterResolver(customConstructorParameterResolverIdentifier);
        }
    }
    #endregion

    private void InitFunctionFactories(IDefaultImplementationBasedObjectFactory defaultImplementationBasedObjectFactory)
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

    private static bool TryResolveSimpleJsonValueSerializer(IDefaultImplementationBasedObjectFactory defaultImplementationBasedObjectFactory,
        Type parameterType, [NotNullWhen(true)] out object? simpleJsonValueSerializer)
    {
        if (parameterType == typeof(ISimpleJsonValueSerializer))
        {
            simpleJsonValueSerializer = new AggregateSimpleJsonValueSerializer(new List<ITypedSimpleJsonValueSerializer>
            {
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedDoubleSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedNullableDoubleSimpleJsonValueSerializer>(),

                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedFloatSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedNullableFloatSimpleJsonValueSerializer>(),

                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedInt16SimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedNullableInt16SimpleJsonValueSerializer>(),

                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedInt32SimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedNullableInt32SimpleJsonValueSerializer>(),

                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedInt64SimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedNullableInt64SimpleJsonValueSerializer>(),

                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedDateTimeSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedNullableDateTimeSimpleJsonValueSerializer>(),

                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedBooleanSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedNullableBooleanSimpleJsonValueSerializer>(),

                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedStringSimpleJsonValueSerializer>()
            });

            return true;
        }

        simpleJsonValueSerializer = null;
        return false;
    }

    private static bool TryResolveJsonConversionSettings(Type parameterType, [NotNullWhen(true)] out object? jsonConversionSettings)
    {
        if (parameterType == typeof(IJsonConversionSettings))
        {
            var conversionErrorTypeConfigurations = new List<ConversionErrorTypeConfiguration>();

            foreach (var conversionErrorType in Enum.GetValues<ConversionErrorType>())
            {
                // Set custom ErrorReportingType for ConversionErrorType here.
                // We report all errors as ErrorReportingType.ReportAsError by default.
                conversionErrorTypeConfigurations.Add(new ConversionErrorTypeConfiguration(conversionErrorType, ErrorReportingType.ReportAsError));
            }

            jsonConversionSettings = new JsonConversionSettings
            {
                JsonPropertyFormat = JsonPropertyFormat.PascalCase,
                FailOnFirstError = true,
                ConversionErrorTypeConfigurations = conversionErrorTypeConfigurations,

                // Set custom interface to implementation mappings here. Default mappings (i.e., IModelClassMapper) will 
                // use try to find an implementation that has the same name space and class name that matches interface name
                // without I. For example for interface JsonQL.Demos.Examples.DataModels.IEmployee implementation  
                // JsonQL.Demos.Examples.DataModels.Employee will be used if it exists.
                TryMapJsonConversionType = null,
            };

            return true;
        }

        jsonConversionSettings = null;
        return false;
    }
}