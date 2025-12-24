==========================
Dependency Injection Setup
==========================

.. contents::
   :local:
   :depth: 2
   
JsonQL uses Dependency Injection (DI) to manage the creation and lifecycle of its core services. This section describes how to configure and register the necessary components to use ``IJsonCompiler`` and ``IQueryManager`` in your applications.


Overview
========

JsonQL is designed to work with standard .NET Dependency Injection containers. The library uses Autofac in its demo project **JsonQL.Demos**, but the principles apply to any DI container that supports constructor injection and singleton lifetimes.

The two main services you'll typically register are:

- `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_: For compiling JSON files with JsonQL expressions
- `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_: For querying JSON data and converting results to C# objects

Core Components
===============

JsonQL Dependency Injection relies on several key components:

**Main Services:**

- `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_: Compiles JSON with JsonQL expressions
- `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_: Queries JSON and converts results to objects

**Factory Pattern:**

- `JsonQL.DependencyInjection.IJsonQLDefaultImplementationBasedObjectFactory <https://github.com/artakhak/JsonQL/blob/main/JsonQL/DependencyInjection/IJsonQLDefaultImplementationBasedObjectFactory.cs>`_: Creates instances using default implementations
- `OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory.IDefaultImplementationBasedObjectFactory <https://github.com/artakhak/OROptimizer.Shared/blob/master/OROptimizer.Shared/ServiceResolver/DefaultImplementationBasedObjectFactory/IDefaultImplementationBasedObjectFactory.cs>`_: Base factory interface for object creation

**Helper Utilities:**

- `JsonQL/DependencyInjection/PropertyDependencyHelper.PropertyDependencyHelper <https://github.com/artakhak/JsonQL/blob/main/JsonQL/DependencyInjection/PropertyDependencyHelper.cs>`_: Sets property dependencies using reflection
- Custom constructor parameter resolvers for specialized dependency injection

Dependency Injection Registration Example
=========================================

The Autofac module class `JsonQL.Demos.Startup.DependencyInjection.JsonQLClassRegistrationsModule <https://github.com/artakhak/JsonQL/blob/main/JsonQL.Demos/Startup/DependencyInjection/JsonQLClassRegistrationsModule.cs>`_ in project demonstrates **JsonQL.Demos** demonstrates using setting up DI registrations for `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_ and `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_. 

**JsonQLClassRegistrationsModule**:

.. raw:: html

   <details>
   <summary>Click to expand <b>JsonQL.Demos.Startup.DependencyInjection.JsonQLClassRegistrationsModule</b></summary>

.. code-block:: csharp

    using System.Diagnostics.CodeAnalysis;
    using Autofac;
    using JsonQL.Compilation;
    using JsonQL.Compilation.JsonFunction;
    using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
    using JsonQL.Compilation.JsonValueLookup;
    using JsonQL.Compilation.UniversalExpressionParserJsonQL;
    using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;
    using JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;
    using JsonQL.DependencyInjection;
    using JsonQL.Query;
    using OROptimizer.Diagnostics.Log;
    using OROptimizer.ServiceResolver;
    using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;
    using Module = Autofac.Module;

    namespace JsonQL.Demos.Startup.DependencyInjection;

    // ReSharper disable once InconsistentNaming
    public class JsonQLClassRegistrationsModule : Module
    {
        private readonly ILog _logger;

        public JsonQLClassRegistrationsModule(ILog logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var jsonFunctionFromExpressionParserDependencies = new List<object>();

            var jsonQlDefaultImplementationBasedObjectFactory = new JsonQLDefaultImplementationBasedObjectFactory(type => true,
                _logger);
            
            jsonQlDefaultImplementationBasedObjectFactory.RegisterCustomConstructorParameterResolvers(
                new CustomConstructorParameterResolver(Guid.NewGuid(),
                    (factory, type, parameterInfo) =>
                    {
                        if (parameterInfo.ParameterType == typeof(ICompilationResultLogger))
                            return (true,
                                  
                                new QueryManagerCompilationResultLogger(
                                    factory.GetOrCreateInstance<ICompilationResultLogger>())
                                );

                        if (parameterInfo.ParameterType == typeof(IJsonQLExpressionLanguageProvider))
                            return (true,
                                new CustomJsonExpressionLanguageProvider(
                                    factory.GetOrCreateInstance<IJsonQLExpressionLanguageProvider>()));

                        if (TryResolveJsonFunctionFactory(factory, jsonFunctionFromExpressionParserDependencies, parameterInfo.ParameterType, out var jsonFunctionFactory))
                            return (true, jsonFunctionFactory);

                        return (false, null);
                    }));

            builder.Register(_ =>
            {
                var jsonCompiler = jsonQlDefaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonCompiler>();

                var jsonFunctionFromExpressionParser = jsonQlDefaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonFunctionFromExpressionParser>();
                foreach (var jsonFunctionFromExpressionParserDependency in jsonFunctionFromExpressionParserDependencies)
                {
                    PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                        jsonFunctionFromExpressionParserDependency, jsonFunctionFromExpressionParser);
                }

                return jsonCompiler;
            }).As<IJsonCompiler>().SingleInstance();

            builder.Register(_ => jsonQlDefaultImplementationBasedObjectFactory.GetOrCreateInstance<IQueryManager>()).As<IQueryManager>().SingleInstance();
        }

        private static T GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser<T>(List<object> jsonFunctionFromExpressionParserDependencies, 
            Func<TryResolveConstructorParameterValueDelegate?,T> createObject) where T: class
        {
            var objectWithDependencyOnJsonFunctionFromExpressionParser = createObject(null);
            jsonFunctionFromExpressionParserDependencies.Add(objectWithDependencyOnJsonFunctionFromExpressionParser);
            return objectWithDependencyOnJsonFunctionFromExpressionParser;
        }
     
        private static bool TryResolveJsonFunctionFactory(IDefaultImplementationBasedObjectFactory defaultImplementationBasedObjectFactory,
            List<object> jsonFunctionFromExpressionParserDependencies,
            Type parameterType, [NotNullWhen(true)] out object? jsonFunctionFactory)
        {

            if (parameterType == typeof(IBracesJsonFunctionFactory))
            {
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBracesJsonFunctionFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomBracesJsonFunctionFactory(defaultJsonFunctionFactory));
                
                return true;
            }

            if (parameterType == typeof(IBinaryOperatorJsonFunctionFactory))
            {
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBinaryOperatorJsonFunctionFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomBinaryOperatorJsonFunctionFactory(defaultJsonFunctionFactory));

                return true;
            }

            if (parameterType == typeof(IUnaryPrefixOperatorJsonFunctionFactory))
            {
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPrefixOperatorJsonFunctionFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomUnaryPrefixOperatorJsonFunctionFactory(defaultJsonFunctionFactory));

                return true;
            }

            if (parameterType == typeof(IUnaryPostfixOperatorJsonFunctionFactory))
            {
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPostfixOperatorJsonFunctionFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomUnaryPostfixOperatorJsonFunctionFactory(defaultJsonFunctionFactory));

                return true;
            }

            if (parameterType == typeof(IJsonValueCollectionItemsSelectorPathElementFactory))
            {
                var jsonValueLookupHelpers = defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonValueLookupHelpers>();
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonValueCollectionItemsSelectorPathElementFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomJsonValueCollectionItemsSelectorPathElementFactory(defaultJsonFunctionFactory, jsonValueLookupHelpers));

                return true;
            }
           
            if (parameterType == typeof(ISpecialLiteralJsonFunctionFactory))
            {
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<ISpecialLiteralJsonFunctionFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomSpecialLiteralJsonFunctionFactory(defaultJsonFunctionFactory));

                return true;
            }

            if (parameterType == typeof(INumericValueJsonFunctionFactory))
            {
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<INumericValueJsonFunctionFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomNumericValueJsonFunctionFactory(defaultJsonFunctionFactory));

                return true;
            }

            if (parameterType == typeof(IConstantTextJsonFunctionFactory))
            {
                var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    defaultImplementationBasedObjectFactory.GetOrCreateInstance<IConstantTextJsonFunctionFactory>);

                jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                    _ => new CustomConstantTextJsonFunctionFactory(defaultJsonFunctionFactory));

                return true;
            }

            jsonFunctionFactory = null;
            return false;
        }
    }


.. raw:: html

   </details><br/><br/>

Using the Services
==================

Once registered, you can inject and use the services:

**Using IJsonCompiler:**

.. code-block:: csharp

    public class MyService
    {
        private readonly IJsonCompiler _jsonCompiler;

        public MyService(IJsonCompiler jsonCompiler)
        {
            _jsonCompiler = jsonCompiler;
        }

        public void CompileJson()
        {
            var jsonData = new JsonTextData(
                "MyJson",
                File.ReadAllText("data.json")
            );

            var result = _jsonCompiler.Compile(jsonData);

            if (result.CompilationErrors.Count == 0)
            {
                // Success - use result.CompiledJsonFiles
            }
        }
    }

**Using IQueryManager:**

.. code-block:: csharp

    public class MyQueryService
    {
        private readonly IQueryManager _queryManager;

        public MyQueryService(IQueryManager queryManager)
        {
            _queryManager = queryManager;
        }

        public List<Employee> GetEmployees()
        {
            var jsonData = new JsonTextData(
                "Data",
                File.ReadAllText("employees.json")
            );

            var result = _queryManager.QueryObject<List<Employee>>(
                "Employees.Where(e => e.Age >= 40)",
                jsonData
            );

            return result.Value;
        }
    }

Lifecycle Management
====================

**Singleton Lifetime (Recommended):**

Both ``IJsonCompiler`` and ``IQueryManager`` should be registered as singletons:

- **Thread-Safe**: Both services are designed to be thread-safe
- **Performance**: Avoids repeated initialization of internal components
- **Memory Efficiency**: Shares cached data across requests

**Scoped/Transient Lifetimes:**

While possible, scoped or transient lifetimes are not recommended:

- Increased overhead from repeated initialization
- Higher memory usage
- No benefits over singleton lifetime for these stateless services

Advanced Configuration
======================

**Custom Factory Settings:**

.. code-block:: csharp

    var objectFactory = new JsonQLDefaultImplementationBasedObjectFactory(
        resolvedTypeInstanceCanBeCached: type => 
        {
            // Return true to cache instances, false for new instances each time
            // Return null to use default caching behavior
            return type.Name.EndsWith("Factory") ? true : (bool?)null;
        },
        logger: _logger
    );

**Property Dependency Injection:**

JsonQL uses ``PropertyDependencyHelper`` for setting property dependencies:

.. code-block:: csharp

    // Automatically sets properties matching the type
    var instanceWithDependencies = PropertyDependencyHelper
        .SetJsonFunctionFromExpressionParser(instance, dependency);

Troubleshooting
===============

**Issue: Missing Dependencies**

If you see errors about missing dependencies:

- Ensure you're using ``IJsonQLDefaultImplementationBasedObjectFactory``
- Call ``InitFunctionFactories()`` before resolving services
- Check that your logger is properly registered

**Issue: Performance Problems**

If compilation/querying is slow:

- Verify services are registered as singletons, not transient
- Check that ``InitFunctionFactories()`` is called only once
- Review your logging configuration (verbose logging can impact performance)

**Issue: Thread Safety Concerns**

Both services are thread-safe when registered as singletons:

- Internal state is immutable or properly synchronized
- No shared mutable state between calls
- Safe to use from multiple threads concurrently

Best Practices
==============

1. **Use Singleton Lifetime**: Register ``IJsonCompiler`` and ``IQueryManager`` as singletons
2. **Use Factory Approach**: Prefer ``IJsonQLDefaultImplementationBasedObjectFactory`` over manual registration
3. **Initialize Once**: Call ``InitFunctionFactories()`` once during application startup
4. **Proper Logging**: Configure a logger for diagnostics and troubleshooting
5. **Container Scope**: Build the DI container once at application startup
6. **Avoid Direct Instantiation**: Always use DI to obtain service instances

Related Interfaces
==================

When working with JsonQL DI:

- `JsonQL.Compilation.IJsonCompiler <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/IJsonCompiler.cs>`_: Main compilation service
- `JsonQL.Query.IQueryManager <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Query/IQueryManager.cs>`_: Main query service
- `JsonQL.DependencyInjection.IJsonQLDefaultImplementationBasedObjectFactory <https://github.com/artakhak/JsonQL/blob/main/JsonQL/DependencyInjection/IJsonQLDefaultImplementationBasedObjectFactory.cs>`_: Factory for creating JsonQL instances
- `OROptimizer.Shared.Diagnostics.Log.ILog <https://github.com/artakhak/OROptimizer.Shared/blob/master/OROptimizer.Shared/Diagnostics/Log/ILog.cs>`_: Logging interface (from **OROptimizer.Shared** library)
- `JsonQL.Compilation.ICompilationResultLogger <https://github.com/artakhak/JsonQL/blob/main/JsonQL/Compilation/ICompilationResultLogger.cs>`_: An interface used for logging compilation results, such as a log message indicating that JSON files were compiled successfully, or logs with error details, if compilation resulted in errors.

See Also
========

- `JsonQL GitHub Repository <https://github.com/artakhak/JsonQL>`_: Source code and examples
- `Autofac Documentation <https://autofac.org/>`_: DI container used in examples
