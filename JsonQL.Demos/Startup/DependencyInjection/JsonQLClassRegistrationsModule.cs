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
        
        var jsonFunctionFromExpressionParser = jsonQlDefaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonFunctionFromExpressionParser>();
        foreach (var jsonFunctionFromExpressionParserDependency in jsonFunctionFromExpressionParserDependencies)
        {
            PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                jsonFunctionFromExpressionParserDependency, jsonFunctionFromExpressionParser);
        }
        
        var jsonCompiler = jsonQlDefaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonCompiler>();
        var jsonQueryManager = jsonQlDefaultImplementationBasedObjectFactory.GetOrCreateInstance<IQueryManager>();

        builder.RegisterInstance(jsonCompiler).As<IJsonCompiler>().SingleInstance();
        builder.RegisterInstance(jsonQueryManager).As<IQueryManager>().SingleInstance();
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
