using Autofac;
using JsonQL.Compilation;
using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctionFactories;
using JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.DependencyInjection;
using JsonQL.JsonToObjectConversion;
using JsonQL.JsonToObjectConversion.Serializers;
using JsonQL.Query;
using OROptimizer.Diagnostics.Log;
using OROptimizer.ServiceResolver.DefaultImplementationBasedObjectFactory;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Module = Autofac.Module;

namespace JsonQL.Demos.Startup.DependencyInjection;

// ReSharper disable once InconsistentNaming
public class IJsonQLClassRegistrationsModule : Module
{
    private readonly ILog _logger;

    public IJsonQLClassRegistrationsModule(ILog logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);

        DefaultImplementationBasedObjectFactory? defaultImplementationBasedObjectFactory = null;

        var jsonFunctionFromExpressionParserDependencies = new List<object>();

        [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
        (bool parameterValueWasResolved, object? resolvedValue) CustomDiResolver(Type type, ParameterInfo parameterInfo)
        {
            if (defaultImplementationBasedObjectFactory == null)
                throw new InvalidOperationException($"The value of [{nameof(defaultImplementationBasedObjectFactory)}] was not set.");

            if (parameterInfo.ParameterType == typeof(ICompilationResultLogger))
                return (true,
                    new QueryManagerCompilationResultLogger(new CompilationResultLogger()));

            if (parameterInfo.ParameterType == typeof(ILog))
                return (true, _logger);

            if (parameterInfo.ParameterType == typeof(IJsonQLExpressionLanguageProvider))
                return (true, 
                    new CustomJsonExpressionLanguageProvider(
                        defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonQLExpressionLanguageProvider>()));

            if (parameterInfo.ParameterType == typeof(IStringFormatter))
                return (true, defaultImplementationBasedObjectFactory.GetOrCreateInstance<IDefaultStringFormatterFactory>().Create());

            if (TryResolveSimpleJsonValueSerializer(defaultImplementationBasedObjectFactory, parameterInfo.ParameterType, out var simpleJsonValueSerializer))
                return (true, simpleJsonValueSerializer);

            if (TryResolveJsonConversionSettings(defaultImplementationBasedObjectFactory, parameterInfo.ParameterType, out var jsonConversionSettings))
                return (true, jsonConversionSettings);
           
            if (TryResolveJsonFunctionFactory(defaultImplementationBasedObjectFactory, jsonFunctionFromExpressionParserDependencies, parameterInfo.ParameterType, out var jsonFunctionFactory))
                return (true, jsonFunctionFactory);

            return (false, null);
        };

        defaultImplementationBasedObjectFactory = new DefaultImplementationBasedObjectFactory(_ => { },
            CustomDiResolver, _ => true, _logger);

        builder.Register(_ =>
        {
            var jsonCompiler = defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonCompiler>();
            
            var jsonFunctionFromExpressionParser = defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonFunctionFromExpressionParser>();
            foreach (var jsonFunctionFromExpressionParserDependency in jsonFunctionFromExpressionParserDependencies)
            {
                PropertyDependencyHelper.SetJsonFunctionFromExpressionParser(
                    jsonFunctionFromExpressionParserDependency, jsonFunctionFromExpressionParser);
            }

            return jsonCompiler;
        }).As<IJsonCompiler>().SingleInstance();

        builder.Register(_ => defaultImplementationBasedObjectFactory.GetOrCreateInstance<IQueryManager>()).As<IQueryManager>().SingleInstance();
    }

    private static T GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser<T>(List<object> jsonFunctionFromExpressionParserDependencies, Func<T> createObject) where T: class
    {
        var objectWithDependencyOnJsonFunctionFromExpressionParser = createObject();
        jsonFunctionFromExpressionParserDependencies.Add(objectWithDependencyOnJsonFunctionFromExpressionParser);
        return objectWithDependencyOnJsonFunctionFromExpressionParser;
    }

    private static bool TryResolveJsonFunctionFactory(DefaultImplementationBasedObjectFactory defaultImplementationBasedObjectFactory,
        List<object> jsonFunctionFromExpressionParserDependencies,
        Type parameterType, [NotNullWhen(true)] out object? jsonFunctionFactory)
    {

        if (parameterType == typeof(IBracesJsonFunctionFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBracesJsonFunctionFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomBracesJsonFunctionFactory(defaultJsonFunctionFactory));
            
            return true;
        }

        if (parameterType == typeof(IBinaryOperatorJsonFunctionFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<IBinaryOperatorJsonFunctionFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomBinaryOperatorJsonFunctionFactory(defaultJsonFunctionFactory));

            return true;
        }

        if (parameterType == typeof(IUnaryPrefixOperatorJsonFunctionFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPrefixOperatorJsonFunctionFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomUnaryPrefixOperatorJsonFunctionFactory(defaultJsonFunctionFactory));

            return true;
        }

        if (parameterType == typeof(IUnaryPostfixOperatorJsonFunctionFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<IUnaryPostfixOperatorJsonFunctionFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomUnaryPostfixOperatorJsonFunctionFactory(defaultJsonFunctionFactory));

            return true;
        }

        if (parameterType == typeof(IJsonValueCollectionItemsSelectorPathElementFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<IJsonValueCollectionItemsSelectorPathElementFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomJsonValueCollectionItemsSelectorPathElementFactory(defaultJsonFunctionFactory));

            return true;
        }
       
        if (parameterType == typeof(ISpecialLiteralJsonFunctionFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<ISpecialLiteralJsonFunctionFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomSpecialLiteralJsonFunctionFactory(defaultJsonFunctionFactory));

            return true;
        }

        if (parameterType == typeof(INumericValueJsonFunctionFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<INumericValueJsonFunctionFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomNumericValueJsonFunctionFactory(defaultJsonFunctionFactory));

            return true;
        }

        if (parameterType == typeof(IConstantTextJsonFunctionFactory))
        {
            var defaultJsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<IConstantTextJsonFunctionFactory>);

            jsonFunctionFactory = GetOrCreateObjectThatDependsOnJsonFunctionFromExpressionParser(jsonFunctionFromExpressionParserDependencies,
                () => new CustomConstantTextJsonFunctionFactory(defaultJsonFunctionFactory));

            return true;
        }

        jsonFunctionFactory = null;
        return false;
    }

    private static bool TryResolveSimpleJsonValueSerializer(DefaultImplementationBasedObjectFactory defaultImplementationBasedObjectFactory,
        Type parameterType, [NotNullWhen(true)] out object? simpleJsonValueSerializer)
    {
        if (parameterType == typeof(ISimpleJsonValueSerializer))
        {
            simpleJsonValueSerializer = new AggregateSimpleJsonValueSerializer(new List<ITypedSimpleJsonValueSerializer>
            {
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedDoubleSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedFloatSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedInt16SimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedInt32SimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedInt64SimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedDateTimeSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedBooleanSimpleJsonValueSerializer>(),
                defaultImplementationBasedObjectFactory.GetOrCreateInstance<TypedStringSimpleJsonValueSerializer>()
            });

            return true;
        }

        simpleJsonValueSerializer = null;
        return false;
    }

    private static bool TryResolveJsonConversionSettings(DefaultImplementationBasedObjectFactory defaultImplementationBasedObjectFactory,
        Type parameterType, [NotNullWhen(true)] out object? jsonConversionSettings)
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